using Lockstep.Math;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class SpawnSystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref SpawnData spawnData, ref AssetData assetData){
            var count = spawnData.Count;
            var center = spawnData.Position;
            var radius = spawnData.Radius;
            var spawnPositions = new NativeArray<LVector3>(count, Allocator.Temp,
                NativeArrayOptions.UninitializedMemory);
            GeneratePoints.RandomPointsInUnitCube(spawnPositions);
            var pointPtr = spawnPositions.GetPointer(0);
            var context = _context;
            for (int i = 0; i < count; ++i, ++pointPtr) {
                var boidPtr = context.PostCmdCreateBoid();
                boidPtr->Transform.Position = center + (*pointPtr * radius);
                boidPtr->Transform.Forward = *pointPtr;
                boidPtr->Transform.Scale = 1;
                boidPtr->Prefab.AssetId = assetData.AssetId;
            }

            spawnPositions.Dispose();
            context.PostCmdDestroyEntity(entity);
        }
    }
}