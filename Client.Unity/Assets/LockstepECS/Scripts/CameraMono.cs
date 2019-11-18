using Lockstep.UnsafeECS.Game;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


namespace Lockstep.Game{
    public class CameraMono : MonoBehaviour {
        public Transform targetTrans;
        public GlobalFog fog;
        public Vector3 scaleOffset;
        public Vector3 offset;
        public float fogStartDist = 20;
        public float fogDistScale = 1.5f;
        public void Awake(){
            //offset = transform.position - targetTrans.position;
            fog = GetComponent<GlobalFog>();
            fog.startDistance = fogStartDist;
        }

        public float lerpSpd = 1;
        public float xRote = 11;
        private void Update(){
            if (!Context.Instance.HasInit) {
                return;
            }

            if (targetTrans == null) {
                var id =GlobalStateService.Instance.LocalActorId;
                var view = EntityViewBoidObstacle.GetView(id);
                targetTrans = view?.transform;
            }
            if(targetTrans == null) return;
            var offsetScale = targetTrans.localScale.x;
            fog.startDistance = fogStartDist * offsetScale * fogDistScale;
            var pos = targetTrans.TransformPoint(offset + offsetScale * scaleOffset);
            //transform.position = Vector3.Lerp(transform.position, pos, 0.3f);
            //var rawRot = transform.rotation;
            //transform.LookAt(targetTrans.position,Vector3.up);
            //var dstRot = transform.rotation;
            //transform.rotation = Quaternion.Lerp(rawRot, dstRot, 0.3f);
            transform.position = pos;
            transform.rotation =transform.rotation;
            if (targetTrans != null) {
                if (transform.parent != targetTrans) {
                    transform.SetParent(targetTrans,true);
                }
            }
        }
    }
}