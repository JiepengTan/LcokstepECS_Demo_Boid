using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Lockstep.Game;
using Lockstep.Math;
using Lockstep.UnsafeECS;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Entity = Lockstep.UnsafeECS.Entity;
using float3 = Lockstep.Math.LVector3;
using float2 = Lockstep.Math.LVector2;
using NativeArrayOptions = Lockstep.UnsafeECS.NativeArrayOptions;

namespace Lockstep.UnsafeECS.Game {
    [StructLayout(LayoutKind.Sequential)]
    [System.Serializable]
    public struct BoidSharedData {
        public LFloat CellRadius;
        public LFloat SeparationWeight;
        public LFloat AlignmentWeight;
        public LFloat TargetWeight;
        public LFloat ObstacleAversionDistance;
        public LFloat MoveSpeed;
    }

    public partial class Context {
        public BoidSharedData BoidSettting;
        public List<Transform> ViewTrans = new List<Transform>();
        public Lockstep.UnsafeECS.NativeArray<LVector3> AllTargets;
        public Lockstep.UnsafeECS.NativeArray<LVector3> AllObstacle;
        public Lockstep.UnsafeECS.NativeArray<CellData> AllCellData;
        public NativeMultiHashMap<int, int> HashMap;
        public bool hasInitMap = false;

        protected void OnDestroy(){
            Debug.LogError("Destoryed !");
            if (AllTargets.Length > 0) {
                AllTargets.Dispose();
            }

            if (AllObstacle.Length > 0) {
                AllObstacle.Dispose();
            }

            if (HashMap.Length > 0) {
                HashMap.Dispose();
            }

            if (hasInitMap) {
                HashMap.Dispose();
            }
        }
    }

    public unsafe class BoidUnityInitSystem : GameBaseSystem, IInitializeSystem {
        public void Initialize(IContext context){
            var spawners = GameObject.FindObjectsOfType<SSSamples.Boids.Authoring.SpawnRandomInSphere>();
            for (int i = 0; i < spawners.Length; i++) {
                var spawner = spawners[i];
                if (spawner.Count <= 0) continue;
                var entity = _context.PostCmdCreateBoidSpawner();
                entity->SpawnerData.Count = spawner.Count;
                entity->SpawnerData.Position = spawner.transform.position.ToLVector3();
                entity->SpawnerData.Radius = spawner.Radius.ToLFloat();
                entity->AssetData.AssetId = i;
            }

            int idx = 0;
            var obstacles = GameObject.FindObjectsOfType<SSSamples.Boids.BoidObstacleProxy>();
            foreach (var item in obstacles) {
                _context.ViewTrans.Add(item.transform);
                var entity = _context.PostCmdCreateBoidObstacle();
                entity->ViewData.ViewId = idx++;
                entity->PositionData.Value = item.transform.position.ToLVector3();
            }

            var targets = GameObject.FindObjectsOfType<SSSamples.Boids.BoidTargetProxy>();
            foreach (var item in targets) {
                _context.ViewTrans.Add(item.transform);
                var entity = _context.PostCmdCreateBoidTarget();
                entity->ViewData.ViewId = idx++;
                entity->PositionData.Value = item.transform.position.ToLVector3();
            }

            var dataMono = GameObject.FindObjectOfType<BoidSetting>();
            _context.BoidSettting = dataMono.data;
        }
    }

    public unsafe partial class BoidSpawnerSystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref SpawnerData spawner, ref AssetData assetData){
            var count = spawner.Count;
            var center = spawner.Position;
            var radius = spawner.Radius;
            var spawnPositions = new NativeArray<LVector3>(count, Allocator.Temp,
                NativeArrayOptions.UninitializedMemory);
            GeneratePoints.RandomPointsInUnitCube(spawnPositions);
            var pointPtr = spawnPositions.GetPointer(0);
            var context = _context;
            for (int i = 0; i < count; ++i, ++pointPtr) {
                var boidPtr = context.PostCmdCreateBoid();
                boidPtr->LocalToWorld.Position = center + (*pointPtr * radius);
                boidPtr->LocalToWorld.Forward = *pointPtr;
                boidPtr->AssetData.AssetId = assetData.AssetId;
            }
            
            spawnPositions.Dispose();
            context.PostCmdDestroyEntity(entity);
        }
    }

    public unsafe partial class BoidViewTransformUpdateSystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref ViewData viewData, ref PositionData positionData){
            var curPosition = _context.ViewTrans[viewData.ViewId].position.ToLVector3();
            positionData.Value = curPosition;
        }
    }

    //TODO
    public unsafe partial class BoidTargetMoveSystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref PositionData positionData){ }
    }

    //TODO
    public unsafe partial class BoidCreateDestroySystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref LocalToWorld localToWorld){ }
    }

    public unsafe partial class BoidCopyStateSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            [ReadOnly] public NativeArray<CellData> cellPtr0;

            public void Execute(int index, [ReadOnly] ref LocalToWorld localToWorld){
                var ptr = cellPtr0.GetPointer(index);
                ptr->Separation = localToWorld.Position;
                ptr->Alignment = localToWorld.Forward;
                ptr->Count = 1;
            }
        }

        protected override void BeforeSchedule(){
            if (_context.AllTargets.Length > 0) {
                _context.AllTargets.Dispose();
            }

            if (_context.AllObstacle.Length > 0) {
                _context.AllObstacle.Dispose();
            }

            var _entities = _context._entities;
            _context.AllTargets =
                _entities.GetAllBoidTarget_PositionData_Filed<LVector3>(_entities.GetOffsetOfPositionData_Value());
            _context.AllObstacle =
                _entities.GetAllBoidObstacle_PositionData_Filed<LVector3>(_entities.GetOffsetOfPositionData_Value());
            if (_context._entities.CurBoidCount == 0) return;
            if (_context.AllCellData.Length != _context._entities.CurBoidCount) {
                if (_context.AllCellData.Length != 0) {
                    _context.AllCellData.Dispose();
                }

                _context.AllCellData = new NativeArray<CellData>(_context._entities.CurBoidCount,
                    Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            }

            JobData.cellPtr0 = _context.AllCellData;
            UnityViewBoidSystem.isInited = true;
            UnityViewBoidSystem.sallBoids = _context._entities._BoidAry._EntityAry;
        }
    }

    public unsafe partial class BoidHashPosSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public NativeMultiHashMap<int, int>.ParallelWriter hashMap;
            public LFloat cellRadius;

            public void Execute(int index, [ReadOnly] ref LocalToWorld localToWorld){
                var floorVal = math.floor(localToWorld.Position / cellRadius);
                var hash = (int) Unity.Mathematics.math.hash(floorVal);
                hashMap.Add(hash, index);
            }
        }

        protected override void BeforeSchedule(){
            if (_context._entities.CurBoidCount == 0) return;
            //if (!_context.hasInitMap || _context.HashMap.Length != _context._entities.CurBoidCount) 
            {
                if (_context.hasInitMap) {
                    _context.HashMap.Dispose();
                }

                _context.hasInitMap = true;
                _context.HashMap = new NativeMultiHashMap<int, int>(_context._entities.CurBoidCount,
                    Unity.Collections.Allocator.Persistent);
            }

            JobData.hashMap = _context.HashMap.AsParallelWriter();
            JobData.cellRadius = _context.BoidSettting.CellRadius;
        }
    }

    public unsafe partial class BoidMergeSystem : GameJobSystem {
        //Unity.Collections.IJobNativeMultiHashMapMergedSharedKeyIndices
        public unsafe partial struct JobDefine {
            [ReadOnly] public NativeArray<LVector3> targetPositions;
            [ReadOnly] public NativeArray<LVector3> obstaclePositions;
            public NativeArray<CellData> cellDatas;

            void NearestPosition(NativeArray<LVector3> targets, LVector3 position,
                out int nearestPositionIndex, out LFloat nearestDistance){
                nearestPositionIndex = 0;
                var ptr = targets.GetPointer(0);
                var len = targets.m_Length;
                nearestDistance = math.lengthsq(position - *ptr);
                ++ptr;
                for (int i = 1; i < len; i++, ++ptr) {
                    var distance = math.lengthsq(position - *ptr);
                    var nearest = distance < nearestDistance;

                    nearestDistance = math.select(nearestDistance, distance, nearest);
                    nearestPositionIndex = math.select(nearestPositionIndex, new LFloat(i), nearest);
                }

                nearestDistance = LMath.Sqrt(nearestDistance);
            }


            // Resolves the distance of the nearest obstacle and target and stores the cell index.
            public void ExecuteFirst(int index){
                var ptr = cellDatas.GetPointer(index);
                var position = ptr->Separation / ptr->Count;

                int obstaclePositionIndex;
                LFloat obstacleDistance;
                NearestPosition(obstaclePositions, position, out obstaclePositionIndex, out obstacleDistance);
                ptr->ObstaclePositionIndex = obstaclePositionIndex;
                ptr->ObstacleDistance = obstacleDistance;

                int targetPositionIndex;
                LFloat targetDistance;
                NearestPosition(targetPositions, position, out targetPositionIndex, out targetDistance);
                ptr->TargetPositionIndex = targetPositionIndex;

                ptr->Index = index;
            }

            // Sums the alignment and separation of the actual index being considered and stores
            // the index of this first value where we're storing the cells.
            public void ExecuteNext(int cellIndex, int index){
                var dstPtr = cellDatas.GetPointer(cellIndex);
                var srcPtr = cellDatas.GetPointer(index);
                dstPtr->Count += 1;
                dstPtr->Alignment = dstPtr->Alignment + srcPtr->Alignment;
                dstPtr->Separation = dstPtr->Separation + srcPtr->Separation;
                srcPtr->Index = cellIndex;
            }
        }

        protected override void BeforeSchedule(){
            //assign jobData info
            HashMap = _context.HashMap;
            JobData.targetPositions = _context.AllTargets;
            JobData.obstaclePositions = _context.AllObstacle;
            JobData.cellDatas = _context.AllCellData;
        }
    }

    public unsafe partial class BoidSteerSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public BoidSharedData settings;
            public LFloat DeltaTime;
            public NativeArray<CellData> cellDatas;
            [ReadOnly] public NativeArray<LVector3> targetPositions;
            [ReadOnly] public NativeArray<LVector3> obstaclePositions;

            public void Execute(int index, ref LocalToWorld localToWorld){
                var forward = localToWorld.Forward;
                var currentPosition = localToWorld.Position;
                var cellIndex = cellDatas.GetPointer(index)->Index;
                var ptr = cellDatas.GetPointer(cellIndex);

                var neighborCount = ptr->Count;
                var alignment = ptr->Alignment;
                var separation = ptr->Separation;
                var nearestObstacleDistance = ptr->ObstacleDistance;
                var nearestObstaclePositionIndex = ptr->ObstaclePositionIndex;
                var nearestTargetPositionIndex = ptr->TargetPositionIndex;
                var nearestObstaclePosition = *(obstaclePositions.GetPointer(nearestObstaclePositionIndex));
                var nearestTargetPosition = *(targetPositions.GetPointer(nearestTargetPositionIndex));

                // steering calculations based on the boids algorithm
                var obstacleSteering = currentPosition - nearestObstaclePosition;
                var avoidObstacleHeading = (nearestObstaclePosition + math.normalizesafe(obstacleSteering)
                                            * settings.ObstacleAversionDistance) - currentPosition;
                var targetHeading = settings.TargetWeight
                                    * math.normalizesafe(nearestTargetPosition - currentPosition);
                var nearestObstacleDistanceFromRadius = nearestObstacleDistance - settings.ObstacleAversionDistance;
                var alignmentResult = settings.AlignmentWeight
                                      * math.normalizesafe((alignment / neighborCount) - forward);
                var separationResult = settings.SeparationWeight
                                       * math.normalizesafe((currentPosition * neighborCount) - separation);
                var normalHeading = math.normalizesafe(alignmentResult + separationResult + targetHeading);
                var targetForward = math.select(normalHeading, avoidObstacleHeading,
                    nearestObstacleDistanceFromRadius < 0);
                var nextHeading = math.normalizesafe(forward + DeltaTime * (targetForward - forward));

                // updates based on the new heading
                localToWorld.Position = localToWorld.Position + (nextHeading * (settings.MoveSpeed * DeltaTime));
                localToWorld.Forward = nextHeading;
            }
        }

        protected override void BeforeSchedule(){
            //assign jobData info
            JobData.DeltaTime = new LFloat(null,30);
            JobData.settings = _context.BoidSettting;
            JobData.obstaclePositions = _context.AllObstacle;
            JobData.targetPositions = _context.AllTargets;
            JobData.cellDatas = _context.AllCellData;
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public unsafe class UnityViewBoidSystem : JobComponentSystem {
#if !USING_UNITY_BOID
        [BurstCompile]
        [RequireComponentTag(typeof(SSSamples.Boids.Boid))]
        unsafe struct CopyStateToUnityView : IJobForEachWithEntity<Unity.Transforms.LocalToWorld, EntityRef> {
            [ReadOnly] public NativeArray<Boid> allBoids;

            public void Execute(Unity.Entities.Entity entity, int index,
                ref Unity.Transforms.LocalToWorld localToWorld, ref EntityRef entityRef){
                var ptr = allBoids.GetPointer(entityRef._index);
                if (ptr->EntityRef == entityRef) {
                    localToWorld = new Unity.Transforms.LocalToWorld {
                        Value = float4x4.TRS(
                            ptr->LocalToWorld.Position.ToVector3(),
                            quaternion.LookRotationSafe(ptr->LocalToWorld.Forward.ToVector3(), Unity.Mathematics.math.up()),
                            new Unity.Mathematics.float3(1.0f, 1.0f, 1.0f)
                        )
                    };
                }
            }
        }
#endif

        public static bool isInited = false;
        public static NativeArray<Boid> sallBoids;
        private EntityQuery m_BoidQuery;
        protected override JobHandle OnUpdate(JobHandle inputDeps){
#if !USING_UNITY_BOID
            var boidCount = m_BoidQuery.CalculateEntityCount();
            if (boidCount != 0 && isInited) {
                var steerJob = new CopyStateToUnityView{allBoids =  sallBoids};
                inputDeps = steerJob.Schedule(m_BoidQuery, inputDeps);
            }
#endif
            return inputDeps;
        }

        protected override void OnCreate(){
#if !USING_UNITY_BOID
            m_BoidQuery = GetEntityQuery(new EntityQueryDesc {
                All = new[] {ComponentType.ReadOnly<SSSamples.Boids.Boid>(), 
                    ComponentType.ReadWrite<EntityRef>(),
                    ComponentType.ReadWrite<Unity.Transforms.LocalToWorld>()
                },
            });
#endif
        }
    }
}