// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System.Collections.Generic;
using Lockstep.UnsafeECSDefine;

namespace Lockstep.UnsafeECSDefine {
    [EntityCount(2)]
    public partial class BoidSpawner : IEntity{
        public Transform3D Transform;
        public Prefab Prefab;
        public SpawnData Spawn;
        public AssetData BoidPrefab;
        public BoidSpawnerTag Tag;
    }
    [EntityCount(1000)]
    public partial class BoidCell : IEntity{
        public CellData Cell;
    }
  
    [EntityCount(2000)]
    public partial class Boid: IEntity,IUpdateViewEntity{
        public Transform3D Transform;
        public Prefab Prefab;
        public BoidState State;
        public BoidTag Tag;
    }

    [EntityCount(2)]
    public partial class BoidTarget: IEntity,IBindViewEntity {
        public Transform3D Transform;
        public Prefab Prefab;
        public TargetMoveInfo MoveInfo;
        public BoidTargetTag Tag;
    }

    [EntityCount(2)]
    public partial class BoidObstacle :IEntity,IBindViewEntity,IUpdateViewEntity{
        public Transform3D Transform;
        public Prefab Prefab;
        public PlayerData Player;
        public SkillData Skill;
        public MoveData Move;
        public BoidObstacleTag Tag;
    }


}