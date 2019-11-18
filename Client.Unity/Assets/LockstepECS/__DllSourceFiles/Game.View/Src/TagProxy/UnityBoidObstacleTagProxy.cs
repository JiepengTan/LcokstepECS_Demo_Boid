using Lockstep.UnsafeECS.Game;
using Unity.Entities;
using UnityEngine;

namespace Lockstep.Game.UnityView {
    [DisallowMultipleComponent] 
    public class UnityBoidObstacleTagProxy : ComponentDataProxy<UnityBoidObstacleTag> { }
}