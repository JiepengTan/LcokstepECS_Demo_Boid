namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class DestroySystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref BoidState boidState){
            if (!boidState.IsDied) return;
            if (!boidState.IsScored) {
                _gameStateService.CurScore++;
                var ptr = _context._entities.GetBoidObstacle(boidState.Killer);
                if (ptr!= null && ptr->IsActive) {
                    ptr->Player.Score++;
                }

                boidState.IsScored = true;
            }
            if (boidState.SinkTimer < 0) {
                _context.PostCmdDestroyEntity(entity);
            }
        }  
        protected override void AfterSchedule(bool isSucc){
            //assign jobData info
            _gameStateService.CurEnemyCount = _context._entities.CurBoidCount;
        }
    }
}