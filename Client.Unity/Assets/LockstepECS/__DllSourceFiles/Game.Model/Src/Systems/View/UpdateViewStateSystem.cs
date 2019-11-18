using Lockstep.Math;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
#if false
namespace Lockstep.UnsafeECS.Game {
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public unsafe class UpdateViewStateSystem : JobComponentSystem {
#if !USING_UNITY_BOID
        [BurstCompile]
        unsafe struct UpdateViewStateBoid : IJobForEachWithEntity<Unity.Transforms.LocalToWorld, EntityRef> {
            [ReadOnly] public NativeArray<Boid> allBoids;

            public void Execute(Unity.Entities.Entity entity, int index,
                ref Unity.Transforms.LocalToWorld localToWorld, ref EntityRef entityRef){
                var ptr = allBoids.GetPointer(entityRef._index);
                if (ptr->EntityRef == entityRef) {
                    localToWorld = new Unity.Transforms.LocalToWorld {
                        Value = float4x4.TRS(
                            ptr->Transform.Position.ToVector3(),
                            quaternion.LookRotationSafe(ptr->Transform.Forward.ToVector3(),
                                Unity.Mathematics.math.up()),
                            new Unity.Mathematics.float3(ptr->Transform.Scale)
                        )
                    };
                }
            }
        }

        [BurstCompile]
        unsafe struct UpdateViewStateBoidObstacle : IJobForEachWithEntity<Unity.Transforms.LocalToWorld, EntityRef> {
            [ReadOnly] public NativeArray<BoidObstacle> allBoidObstacles;

            public void Execute(Unity.Entities.Entity entity, int index,
                ref Unity.Transforms.LocalToWorld localToWorld, ref EntityRef entityRef){
                var ptr = allBoidObstacles.GetPointer(entityRef._index);
                if (ptr->EntityRef == entityRef) {
                    localToWorld = new Unity.Transforms.LocalToWorld {
                        Value = float4x4.TRS(
                            ptr->Transform.Position.ToVector3(),
                            quaternion.LookRotationSafe(ptr->Transform.Forward.ToVector3(),
                                Unity.Mathematics.math.up()),
                            new Unity.Mathematics.float3(ptr->Transform.Scale)
                        )
                    };
                }
            }
        }

#endif

        public static bool isInited = false;
        private EntityQuery m_BoidQuery;
        private EntityQuery m_BoidObsQuery;
        public NativeArray<Boid> _allBoids;
        public NativeArray<BoidObstacle> _allBoidObstacles;

        protected override JobHandle OnUpdate(JobHandle inputDeps){
#if !USING_UNITY_BOID
            if (isInited) {
                {
                    var ary = Context.Instance._entities._BoidAry;
                    CopyState(ref ary,ref _allBoids);
                    if (ary.Length != 0) {
                        var steerJob = new UpdateViewStateBoid {
                            allBoids = _allBoids,
                        };
                        inputDeps = steerJob.Schedule(m_BoidQuery, inputDeps);
                    }
                }
                {
                    var ary = Context.Instance._entities._BoidObstacleAry;
                    CopyState(ref ary,ref _allBoidObstacles);
                    if (ary.Length != 0) {
                        var steerJob = new UpdateViewStateBoidObstacle {
                            allBoidObstacles = _allBoidObstacles,
                        };
                        inputDeps = steerJob.Schedule(m_BoidObsQuery, inputDeps);
                    }
                }
            }
#endif

            return inputDeps;
        }

        private void CopyState<T>(ref NativeEntityArray<T> ary,ref NativeArray<T> _allBoids )where T : unmanaged, IEntity{
            if (ary.Length != 0) {
                if (_allBoids.Length == ary._EntityAry.Length) {
                    NativeArray<T>.Copy(ary._EntityAry, _allBoids);
                }
                else {
                    if (_allBoids.Length != 0) {
                        _allBoids.Dispose();
                    }
                    _allBoids = new NativeArray<T>(ary._EntityAry, Allocator.Persistent);
                }
            }
            else {
                if (_allBoids.Length != 0) {
                    _allBoids.Dispose();
                }
            }
        }

        protected override void OnCreate(){
#if !USING_UNITY_BOID
            m_BoidQuery = GetEntityQuery(new EntityQueryDesc {
                All = new[] {
                    ComponentType.ReadOnly<Lockstep.Game.UnityView.UnityBoidTag>(),
                    ComponentType.ReadWrite<EntityRef>(),
                    ComponentType.ReadWrite<Unity.Transforms.LocalToWorld>()
                },
            });
            m_BoidObsQuery = GetEntityQuery(new EntityQueryDesc {
                All = new[] {
                    ComponentType.ReadOnly<Lockstep.Game.UnityView.UnityBoidObstacleTag>(),
                    ComponentType.ReadWrite<EntityRef>(),
                    ComponentType.ReadWrite<Unity.Transforms.LocalToWorld>()
                },
            });
#endif
        }
    }
}
#endif