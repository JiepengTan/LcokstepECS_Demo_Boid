using Lockstep.Math;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class SkillSystem : GameExecuteSystem {
        private LFloat _deltaTime;
        public void Execute(Entity* ptr,ref SkillData skillData){
            //skillData.IsFiring = skillMonoData.IsFiring;
            if (skillData.IsNeedFire) {
                if (skillData.CdTimer <= 0 && !skillData.IsFiring) {
                    skillData.CdTimer = skillData.CD;
                    skillData.IsFiring = true;
                    skillData.DurationTimer = skillData.Duration;
                    RaiseEventOnSkillFire(ptr,ref skillData);
                }
            }

            skillData.CdTimer -= _deltaTime;
            if (skillData.IsFiring) {
                skillData.DurationTimer -= _deltaTime;
                if (skillData.DurationTimer <= 0) {
                    skillData.IsFiring = false;
                    RaiseEventOnSkillDone(ptr,ref skillData);
                }
            }

            skillData.IsNeedFire = false;
        }
        protected override bool BeforeSchedule(){
            //assign jobData info
            _deltaTime = _globalStateService.DeltaTime;
            return true;
        }
    }
}