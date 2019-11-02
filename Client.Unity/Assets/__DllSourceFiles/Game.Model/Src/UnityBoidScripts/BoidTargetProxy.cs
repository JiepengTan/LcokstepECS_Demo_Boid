using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace SSSamples.Boids
{
    public struct BoidTarget : IComponentData
    { }
    
    [DisallowMultipleComponent] 
    public class BoidTargetProxy : ComponentDataProxy<BoidTarget> { }
}