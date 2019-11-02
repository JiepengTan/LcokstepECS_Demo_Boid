namespace Lockstep.Game {
    [System.Serializable]
    public enum ECampType  {
        Player,
        Enemy,
        Other,
    }
    public enum EDir {
        Up =0 ,
        Left = 1,
        Down = 2,
        Right = 3,
        EnumCount = 4,
    }
    
    public enum EItemType {
        AddLife,
        Boom,
        Upgrade,
        EnumCount,
    }
   
}