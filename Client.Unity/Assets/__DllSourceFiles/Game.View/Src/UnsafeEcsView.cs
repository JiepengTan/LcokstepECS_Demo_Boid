using Lockstep.Math;
using Lockstep.UnsafeECS;
using Lockstep.UnsafeECS.Game;
using Lockstep.UnsafeECS;
using UnityEngine;

namespace Lockstep.Game {
    public unsafe class UnsafeEcsView : MonoBehaviour {
        public Entity* ptr;

        public void Update(){
            if (ptr != null) {
                if (ptr->TypeId != EntityIds.Enemy) {
                    return;
                }

                var p = (Enemy*) ptr;
                transform.position = p->Transform2D.pos.ToVector3XZ();
            }
        }
    }
}