using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;

namespace Lockstep.UnsafeECS.Game {
    public static class TransformUtil {
        public static void CopyFrom(ref this Transform3D transData , Transform transform){
            transData.Position = transform.position.ToLVector3();
            transData.Forward = transform.forward.ToLVector3();
            transData.Scale = transform.localScale.x.ToLFloat();
        }
    }

   

    public unsafe class InitSystem : GameBaseSystem, IInitializeSystem {
        public void Initialize(IContext context){
            var config = _gameConfigService;
            foreach (var spawner in config.SpawnInfos) {
                if (spawner.Count <= 0) continue;
                var entity = _context.PostCmdCreateBoidSpawner();
                entity->Spawn.Count = spawner.Count;
                entity->Spawn.Position = spawner.Position;
                entity->Spawn.Radius = spawner.Radius;
                entity->BoidPrefab.AssetId = spawner.AssetId;
            }

            int playerId = 0;
            var count = _globalStateService.ActorCount;
            for (int i = 0; i < count; i++) {
                var obstacleInfo = config.ObstacleInfos[i % config.ObstacleInfos.Count];
                var entity = _context.PostCmdCreateBoidObstacle();
                entity->Transform.Position = obstacleInfo.Position;
                entity->Transform.Forward = obstacleInfo.Forward;
                entity->Transform.Scale = obstacleInfo.Scale ;
                entity->Skill = obstacleInfo.SkillData;
                entity->Move = obstacleInfo.MoveData;
                entity->Prefab.AssetId = obstacleInfo.AssetId;
                entity->Player.LocalId = playerId++;//
                Debug.Log("Create ObstacleInfos " + entity->Player.LocalId);
            }
            
            foreach (var target in config.TargetInfos) {
                var entity = _context.PostCmdCreateBoidTarget();
                entity->Transform.Position = target.MoveInfo.GetPosition(0);
                entity->Transform.Scale = 1;
                entity->MoveInfo = target.MoveInfo;
                entity->Prefab.AssetId = target.AssetId;
            }
            
            foreach (var spawner in config.SpawnInfos) {
                _gameStateService.CurEnemyCount += spawner.Count;
            }
        }
    }
}