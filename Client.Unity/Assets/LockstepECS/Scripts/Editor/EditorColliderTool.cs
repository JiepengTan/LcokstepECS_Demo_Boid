using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lockstep.Collision2D;
using Lockstep.Math;
using Lockstep.Serialization;
using UnityEditor;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;

[CustomEditor(typeof(ColliderToolMono))]
public class EditorColliderTool : UnityEditor.Editor {
    private ColliderToolMono owner;
    private List<string> names = new List<string>();
    private string testName = "Refinery";


    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        owner = (target as ColliderToolMono);
        if (GUILayout.Button("Generate ColliderDatas")) {
            while (owner.transform.childCount > 0) {
                var c = owner.transform.GetChild(0);
                GameObject.DestroyImmediate(c.gameObject);
            }

            var uColliders = GameObject.FindObjectsOfType<Collider>();
            var allData = GatherData(uColliders);
            ColliderDataUtil.SaveToFile(owner.FilePath, allData);
            var dataAry = ColliderDataUtil.ReadFromFile(owner.FilePath);
            ShowDatas(dataAry);
        }
    }

    List<ColliderData> GatherData(Collider[] uColliders){
        var allData = new List<ColliderData>();
        names.Clear();
        foreach (var collider in uColliders) {
            if(!collider.enabled) continue;
            names.Add(collider.name);
            var scale = collider.transform.localScale;
            var data = new ColliderData();
      

            if (collider is BoxCollider) {
                if (collider.name == testName) {
                    int i = 0;
                }
                var col = collider as BoxCollider;
                data.pos = (col.transform.TransformPoint(col.center)).ToLVector2XZ();
                data.y = (col.transform.position.y + col.center.y  * scale.y).ToLFloat();
                //var val = col.transform.TransformVector(col.size).ToLVector3();
                var val = col.size;
                data.size =  new Vector2(val.x *scale.x,val.z *scale.z).ToLVector2() * LFloat.half;
                data.high = (col.size.y  * scale.y).ToLFloat();
                data.deg = CTransform2D.ToDeg(col.transform.forward.ToLVector2XZ());
            }
            else if (collider is SphereCollider) {
                var col = collider as SphereCollider;
                data.pos = (col.transform.TransformPoint(col.center)).ToLVector2XZ();
                data.y = (col.transform.position.y + col.center.y  * scale.y).ToLFloat();
                
                data.radius = col.radius.ToLFloat() * scale.x.ToLFloat();
                data.high = (col.radius * 2 * scale.y).ToLFloat();
            }
            else if (collider is CapsuleCollider) {
                var col = collider as CapsuleCollider;
                data.pos = (col.transform.TransformPoint(col.center)).ToLVector2XZ();
                data.y = (col.transform.position.y + col.center.y  * scale.y).ToLFloat();
                
                data.radius = col.radius.ToLFloat() * scale.x.ToLFloat();
                data.high = (col.height * 2 * scale.y).ToLFloat();
            }
            else {
               UnityEngine .Debug.LogError($"{ collider.name} unknow colliderType { collider.GetType().Name}");
               continue;
            }
            allData.Add(data);
        }

        return allData;
    }

    private void ShowDatas(ColliderData[] allData){
        var trans = new GameObject("allColliders").transform;
        trans.SetParent(owner.transform, false);
        int idx = 0;
        foreach (var data in allData) {
            var name = names[idx++];
            var go = new GameObject("__ColProxy" + idx + " " + name);
            if (name == testName) {
                int i = 0;
            }

            go.transform.SetParent(trans, false);
            if (data.IsCircle()) {
                var col = go.AddComponent<CapsuleCollider>();
                go.transform.position = data.pos.ToVector3XZ(data.y );
                col.height = (data.high / 2).ToFloat();
                col.radius = data.radius.ToFloat();
            }
            else {
                var col = go.AddComponent<BoxCollider>();
                col.size = data.size.ToVector3XZ(data.high / 2) * 2;
                go.transform.position = data.pos.ToVector3XZ(data.y);
                go.transform.rotation = Quaternion.Euler(0, data.deg.ToFloat(), 0);
            }
        }
    }
}