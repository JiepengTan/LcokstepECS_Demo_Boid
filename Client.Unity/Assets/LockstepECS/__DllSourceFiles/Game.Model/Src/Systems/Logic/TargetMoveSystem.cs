using Lockstep.Math;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class TargetMoveSystem : GameExecuteSystem {
        private LFloat _timer;
        private LVector3 InitPos;

        protected override bool BeforeSchedule(){
            //assign jobData info
            _timer += _globalStateService.DeltaTime;
            return true;
        }

        public void Execute(ref Transform3D transform3D, ref TargetMoveInfo moveInfo,
            ref BoidTargetTag tag){
            var pos = moveInfo.GetPosition(_timer);
            transform3D.Forward = (pos - transform3D.Position).normalized;
            transform3D.Position = pos;
        }
    }
}