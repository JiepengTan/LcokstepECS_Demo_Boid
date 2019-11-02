using System.Collections.Generic;
using SSSamples.Boids;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Allocator = Unity.Collections.Allocator;
using Entity = Unity.Entities.Entity;
using math = Unity.Mathematics.math;

#if USING_UNITY_BOID
namespace Samples.Common {
    public struct GeneratePoints {
        struct PointsInSphere : IJob {
            public float Radius;
            public float3 Center;
            public Unity.Collections.NativeArray<float3> Points;

            public void Execute(){
                var radiusSquared = Radius * Radius;
                var pointsFound = 0;
                var count = Points.Length;
                var random = new Random(0x6E624EB7u);

                while (pointsFound < count) {
                    var p = random.NextFloat3() * new float3(Radius + Radius) - new float3(Radius);
                    if (math.lengthsq(p) < radiusSquared) {
                        Points[pointsFound] = Center + p;
                        pointsFound++;
                    }
                }
            }
        }

        public static JobHandle RandomPointsInSphere(float3 center, float radius, NativeArray<float3> points,
            JobHandle inputDeps){
            var pointsInSphereJob = new PointsInSphere {
                Radius = radius,
                Center = center,
                Points = points
            };
            var pointsInSphereJobHandle = pointsInSphereJob.Schedule(inputDeps);
            return pointsInSphereJobHandle;
        }

        public static void RandomPointsInSphere(float3 center, float radius, NativeArray<float3> points){
            var randomPointsInSphereJobHandle = RandomPointsInSphere(center, radius, points, new JobHandle());
            randomPointsInSphereJobHandle.Complete();
        }

        public static void RandomPointsInUnitSphere(NativeArray<float3> points){
            var randomPointsInSphereJobHandle = RandomPointsInSphere(0.0f, 1.0f, points, new JobHandle());
            randomPointsInSphereJobHandle.Complete();
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    public class SpawnRandomInSphereSystem : ComponentSystem {
        protected override void OnUpdate(){
            Entities.ForEach((Entity e, SpawnRandomInSphere spawner, ref LocalToWorld localToWorld) => {
                int toSpawnCount = spawner.Count;

                // Using a .TempJob instead of a .Temp for `spawnPositions`, because the method
                // `RandomPointsInUnitSphere` passes this NativeArray into a Job
                var spawnPositions = new Unity.Collections.NativeArray<float3>(toSpawnCount, Allocator.TempJob);
                GeneratePoints.RandomPointsInUnitSphere(spawnPositions);

                // Calling Instantiate once per spawned Entity is rather slow, and not recommended
                // This code is placeholder until we add the ability to bulk-instantiate many entities from an ECB
                var entities = new Unity.Collections.NativeArray<Entity>(toSpawnCount, Allocator.Temp);
                for (int i = 0; i < toSpawnCount; ++i) {
                    entities[i] = PostUpdateCommands.Instantiate(spawner.Prefab);
                }

                for (int i = 0; i < toSpawnCount; i++) {
                    PostUpdateCommands.SetComponent(entities[i], new LocalToWorld {
                        Value = float4x4.TRS(
                            localToWorld.Position + (spawnPositions[i] * spawner.Radius),
                            quaternion.LookRotationSafe(spawnPositions[i], math.up()),
                            new float3(1.0f, 1.0f, 1.0f))
                    });
                }

                // Using 'RemoveComponent' instead of 'DestroyEntity' as a safety.
                // Removing the SpawnRandomInSphere component is sufficient to prevent the spawner
                // from executing its spawn logic more than once. The spawner may have other components
                // that are relevant to ongoing processing; this system doesn't know about them & shouldn't
                // assume the entity is safe to delete.
                PostUpdateCommands.RemoveComponent<SpawnRandomInSphere>(e);

                spawnPositions.Dispose();
                entities.Dispose();
            });
        }
    }
}
#endif