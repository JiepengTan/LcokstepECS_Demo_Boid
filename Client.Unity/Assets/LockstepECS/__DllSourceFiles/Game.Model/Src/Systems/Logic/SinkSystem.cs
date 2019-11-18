using Lockstep.Game;
using Lockstep.Math;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class SinkSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            [ReadOnly] public LVector3 SinkOffset;
            [ReadOnly] public LFloat DeltaTime;

            public void Execute(ref Transform3D transform3D, ref BoidState boidState){
                if (!boidState.IsDied) return;
                boidState.SinkTimer -= DeltaTime;
                transform3D.Position += SinkOffset;
            }
        }

        protected override bool BeforeSchedule(){
            //assign jobData info
            JobData.DeltaTime = _globalStateService.DeltaTime;
            JobData.SinkOffset = new LVector3(0,_gameConfigService.BoidSettting.SinkSpd * JobData.DeltaTime,0) ;
            return true;
        }
    }
}