using Lockstep.Game;
using Lockstep.UnsafeECS;

namespace Lockstep.UnsafeECS.Game {
    public class GameLogicSystems : Systems {
        public GameLogicSystems(Context contexts, IServiceContainer services){
            //Init 
            {
                Add(new InitSystem().Init(contexts, services));
            }
            //Input 
            {
                Add(new InputSystem().Init(contexts, services));
            }
            //Update
            {
                Add(new SpawnSystem().Init(contexts, services));
                //boid
                {
                    Add(new BoidCopyStateSystem().Init(contexts, services));
                    Add(new BoidHashPosSystem().Init(contexts, services));
                    Add(new BoidMergeSystem().Init(contexts, services));
                    Add(new BoidSteerSystem().Init(contexts, services));
                }
                //
                Add(new PlayerMoveSystem().Init(contexts, services));
                Add(new TargetMoveSystem().Init(contexts, services));
                Add(new CollisionSystem().Init(contexts, services));
                Add(new SkillSystem().Init(contexts, services));
                Add(new SinkSystem().Init(contexts, services));
                Add(new ScaleSystem().Init(contexts, services));
            }
            //Clean
            {
                Add(new DestroySystem().Init(contexts, services));
            }
        }
    }
}