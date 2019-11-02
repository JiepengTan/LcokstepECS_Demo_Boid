using Lockstep.Game;
using Lockstep.UnsafeECS;

namespace Lockstep.UnsafeECS.Game {
    public class GameLogicSystems : Systems {
        public GameLogicSystems(Context contexts, IServiceContainer services){
            Add(new BoidUnityInitSystem().Init(contexts, services));
            Add(new BoidSpawnerSystem().Init(contexts, services));
            Add(new BoidViewTransformUpdateSystem().Init(contexts, services));
            Add(new BoidCopyStateSystem().Init(contexts, services));
            Add(new BoidHashPosSystem().Init(contexts, services));
            Add(new BoidMergeSystem().Init(contexts, services));
            Add(new BoidSteerSystem().Init(contexts, services));
        }
    }
}