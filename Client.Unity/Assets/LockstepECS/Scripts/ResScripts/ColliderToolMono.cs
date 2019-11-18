using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ColliderToolMono : MonoBehaviour {
    public string dir;
    public string fileName;
    public string FilePath => Path.Combine(Application.dataPath, dir, fileName);
}