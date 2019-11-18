using Lockstep.Math;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class BoidHashPosSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public NativeMultiHashMap<int, int>.ParallelWriter HashMap;
            public LFloat CellRadius;

            public void Execute(int index, [ReadOnly] ref Transform3D transform3D, ref BoidTag tag){
                var floorVal = math.floor(transform3D.Position / CellRadius);
                var hash = (int) Unity.Mathematics.math.hash(floorVal);
                HashMap.Add(hash, index);
            }
        }

        protected override bool BeforeSchedule(){
            if (_context.CurBoidCount == 0) return false;
            JobData.HashMap = _tempFields.CreateHashMap().AsParallelWriter();
            JobData.CellRadius = _gameConfigService.BoidSettting.CellRadius;
            return true;
        }
    }
}