using System.Runtime.CompilerServices;
using Lockstep.Math;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Random = Lockstep.Math.Random;

namespace Lockstep.UnsafeECS {
    
    public static class math {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat lengthsq(LVector3 vec){
            return LMath.Dot(vec, vec);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LFloat select(LFloat a, LFloat b, bool c){
            return c ? b : a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector3 select(LVector3 a, LVector3 b, bool c){
            return c ? b : a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LVector3 normalizesafe(LVector3 x){
            LFloat len = LMath.Dot(x, x);
            if (len > LFloat.EPSILON) {
                return x * (LFloat.one / LMath.Sqrt(len));
            }
            return LVector3.zero;
        }

        public static int3 floor(LVector3 vec){
            return new int3(
                LMath.FloorToInt(vec.x),
                LMath.FloorToInt(vec.y),
                LMath.FloorToInt(vec.z)
            );
        }
    }
    
    public unsafe struct GeneratePoints {
        [BurstCompile]
        struct PointsInSphere : IJob {
            public LFloat Radius;
            public LVector3 Center;
            public Lockstep.UnsafeECS.NativeArray<LVector3> Points;

            public void Execute(){
                var radiusSquared = Radius * Radius;
                var pointsFound = 0;
                var count = Points.Length;
                var ptr = Points.GetPointer(0);
                var random = new Random(0x6E624EB7u);
                var Radius2 = Radius + Radius;
                var radius2Vec = new LVector3(Radius2 , Radius2 , Radius2);
                var radiusVec = new LVector3(Radius, Radius, Radius);
                
                while (pointsFound < count) {
                    var p = random.NextVector3() * radius2Vec - radiusVec;
                    if (math.lengthsq(p) < radiusSquared) {
                        *ptr = Center + p;
                        pointsFound++;
                        ++ptr;
                    }
                }
            }
        }
        [BurstCompile]
        struct PointsInCube : IJob {
            public LFloat Radius;
            public LVector3 Center;
            public Lockstep.UnsafeECS.NativeArray<LVector3> Points;

            public void Execute(){
                var count = Points.Length;
                var ptr = Points.GetPointer(0);
                var random = new Random(0x6E624EB7u);
                var radius2 = Radius + Radius;
                var radius2Vec = new LVector3(radius2 , radius2 , radius2);
                var radiusVec = new LVector3(Radius, Radius, Radius);
                var minPointer = Center - radiusVec;
                for (int i = 0; i < count; ++i,++ptr) {
                    *ptr =  minPointer + random.NextVector3() * radius2Vec;
                }
            }
        }

        public static JobHandle RandomPointsInSphere(LVector3 center, LFloat radius, Lockstep.UnsafeECS.NativeArray<LVector3> points,
            JobHandle inputDeps){
            var pointsInSphereJob = new PointsInSphere {
                Radius = radius,
                Center = center,
                Points = points
            };
            var pointsInSphereJobHandle = pointsInSphereJob.Schedule(inputDeps);
            return pointsInSphereJobHandle;
        }
        public static JobHandle RandomPointsInCube(LVector3 center, LFloat radius, Lockstep.UnsafeECS.NativeArray<LVector3> points,
            JobHandle inputDeps){
            var pointsInSphereJob = new PointsInCube {
                Radius = radius,
                Center = center,
                Points = points
            };
            var pointsInSphereJobHandle = pointsInSphereJob.Schedule(inputDeps);
            return pointsInSphereJobHandle;
        }
        public static void RandomPointsInSphere(LVector3 center, LFloat radius, Lockstep.UnsafeECS.NativeArray<LVector3> points){
            var randomPointsInSphereJobHandle = RandomPointsInSphere(center, radius, points, new JobHandle());
            randomPointsInSphereJobHandle.Complete();
        }
        public static void RandomPointsInUnitSphere(Lockstep.UnsafeECS.NativeArray<LVector3> points){
            var randomPointsInSphereJobHandle = RandomPointsInSphere(LVector3.zero, LFloat.one, points, new JobHandle());
            randomPointsInSphereJobHandle.Complete();
        }
        
        public static void RandomPointsInCube(LVector3 center, LFloat radius, Lockstep.UnsafeECS.NativeArray<LVector3> points){
            var randomPointsInSphereJobHandle = RandomPointsInSphere(center, radius, points, new JobHandle());
            randomPointsInSphereJobHandle.Complete();
        }
        public static void RandomPointsInUnitCube(Lockstep.UnsafeECS.NativeArray<LVector3> points){
            var randomPointsInSphereJobHandle = RandomPointsInSphere(LVector3.zero, LFloat.one, points, new JobHandle());
            randomPointsInSphereJobHandle.Complete();
        }
    }
}