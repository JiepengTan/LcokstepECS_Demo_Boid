using Lockstep.Math;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class BoidCopyStateSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            [ReadOnly] public NativeArray<CellData> CellAry;

            public void Execute(int index, [ReadOnly] ref Transform3D transform3D,[ReadOnly] ref BoidTag Tag){
                var ptr = CellAry.GetPointer(index);
                ptr->Separation = transform3D.Position;
                ptr->Alignment = transform3D.Forward;
                ptr->Count = 1;
            }
        }

        protected override bool BeforeSchedule(){
            if (_context._entities.CurBoidCount == 0) return false;
            JobData.CellAry = _tempFields.AllCellData;
            _context.HasInit = true;
            return true;
        }
    }
}