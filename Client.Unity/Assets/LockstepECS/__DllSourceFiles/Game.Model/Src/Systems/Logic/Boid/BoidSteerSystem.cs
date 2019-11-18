using Lockstep.Game;
using Lockstep.Math;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class BoidSteerSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public ConfigBoidSharedData Settings;
            public LFloat DeltaTime;
            public NativeArray<CellData> CellDatas;
            [ReadOnly] public NativeArray<LVector3> TargetPositions;
            [ReadOnly] public NativeArray<LVector3> ObstaclePositions;

            public void Execute(int index, ref Transform3D transform3D, ref BoidState boidState){
                if(boidState.IsDied) return;
                var forward = transform3D.Forward;
                var currentPosition = transform3D.Position;
                var cellIndex = CellDatas.GetPointer(index)->Index;
                var ptr = CellDatas.GetPointer(cellIndex);

                var neighborCount = ptr->Count;
                var alignment = ptr->Alignment;
                var separation = ptr->Separation;
                var nearestObstacleDistance = ptr->ObstacleDistance;
                var nearestObstaclePositionIndex = ptr->ObstaclePositionIndex;
                var nearestTargetPositionIndex = ptr->TargetPositionIndex;
                var nearestObstaclePosition = *(ObstaclePositions.GetPointer(nearestObstaclePositionIndex));
                var nearestTargetPosition = *(TargetPositions.GetPointer(nearestTargetPositionIndex));

                // steering calculations based on the boids algorithm
                var obstacleSteering = currentPosition - nearestObstaclePosition;
                var avoidObstacleHeading = (nearestObstaclePosition + math.normalizesafe(obstacleSteering)
                                            * Settings.ObstacleAversionDistance) - currentPosition;
                var targetHeading = Settings.TargetWeight
                                    * math.normalizesafe(nearestTargetPosition - currentPosition);
                var nearestObstacleDistanceFromRadius = nearestObstacleDistance - Settings.ObstacleAversionDistance;
                var alignmentResult = Settings.AlignmentWeight
                                      * math.normalizesafe((alignment / neighborCount) - forward);
                var separationResult = Settings.SeparationWeight
                                       * math.normalizesafe((currentPosition * neighborCount) - separation);
                var normalHeading = math.normalizesafe(alignmentResult + separationResult + targetHeading);
                var targetForward = math.select(normalHeading, avoidObstacleHeading,
                    nearestObstacleDistanceFromRadius < 0);
                var nextHeading = math.normalizesafe(forward + DeltaTime * (targetForward - forward));

                // updates based on the new heading
                transform3D.Position = transform3D.Position + (nextHeading * (Settings.MoveSpeed * DeltaTime));
                transform3D.Forward = nextHeading;
            }
        }

        protected override bool BeforeSchedule(){
            if (_context.CurBoidCount == 0) return false;
            //assign jobData info
            JobData.DeltaTime = _globalStateService.DeltaTime;
            JobData.Settings = _gameConfigService.BoidSettting;
            JobData.ObstaclePositions = _tempFields.AllObstaclePos;
            JobData.TargetPositions = _tempFields.AllTargetPos;
            JobData.CellDatas = _tempFields.AllCellData;
            return true;
        }
    }
}