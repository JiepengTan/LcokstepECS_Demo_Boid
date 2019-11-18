using Lockstep.Math;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class BoidMergeSystem : GameJobSystem {
        //Unity.Collections.IJobNativeMultiHashMapMergedSharedKeyIndices
        public unsafe partial struct JobDefine {
            [ReadOnly] public NativeArray<LVector3> TargetPositions;
            [ReadOnly] public NativeArray<LVector3> ObstaclePositions;
            public NativeArray<CellData> CellDatas;

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
                var ptr = CellDatas.GetPointer(index);
                var position = ptr->Separation / ptr->Count;

                NearestPosition(ObstaclePositions, position, out var obstaclePositionIndex, out var obstacleDistance);
                ptr->ObstaclePositionIndex = obstaclePositionIndex;
                ptr->ObstacleDistance = obstacleDistance;

                NearestPosition(TargetPositions, position, out var targetPositionIndex, out _);
                ptr->TargetPositionIndex = targetPositionIndex;

                ptr->Index = index;
            }

            // Sums the alignment and separation of the actual index being considered and stores
            // the index of this first value where we're storing the cells.
            public void ExecuteNext(int cellIndex, int index){
                var dstPtr = CellDatas.GetPointer(cellIndex);
                var srcPtr = CellDatas.GetPointer(index);
                dstPtr->Count += 1;
                dstPtr->Alignment = dstPtr->Alignment + srcPtr->Alignment;
                dstPtr->Separation = dstPtr->Separation + srcPtr->Separation;
                srcPtr->Index = cellIndex;
            }
        }

        protected override bool BeforeSchedule(){
            if (_context.CurBoidCount == 0) return false;
            //assign jobData info
            HashMap = _tempFields.GetHashMap();
            JobData.TargetPositions = _tempFields.AllTargetPos;
            JobData.ObstaclePositions = _tempFields.AllObstaclePos;
            JobData.CellDatas = _tempFields.AllCellData;
            return true;
        }
    }
}