using UnityEngine;

public class TileMapHelper : MonoBehaviour, ITileMapHelper {
    /// <summary>
    /// 是否是碰撞图层
    /// </summary>
    public bool isCollider = true;

    /// <summary>
    /// 是否只用于辅助
    /// </summary>
    public bool isTagMap;

    public bool IsCollider {
        get => isCollider;
        set => isCollider = value;
    }

    public bool IsTagMap {
        get => isTagMap;
        set => isTagMap = value;
    }
}