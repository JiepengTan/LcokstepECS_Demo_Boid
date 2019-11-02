using System.Collections.Generic;
using Lockstep.Math;
using Lockstep.UnsafeECS.Game;
using Lockstep.UnsafeECS;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;
using Unity.Entities;
using Unity.Mathematics;
using math = Lockstep.UnsafeECS.math;

namespace Lockstep.Game {
    public unsafe partial class UnityEntityService : PureEntityService {
        private Dictionary<int, Unity.Entities.Entity> _id2GameObject = new Dictionary<int, Unity.Entities.Entity>();

        //private GameObject[] _prefabs;
        private Unity.Entities.EntityManager _entityManager;
        private Unity.Entities.Entity[] _entityPrefabs;

        public override void OnEntityCreated(Context f, Lockstep.UnsafeECS.Entity* pEntity){
            if (pEntity == null) {
                int i = 0;
                Debug.LogError("OnEntityCreated null");
                return;
            }

            if (pEntity->TypeId != EntityIds.Boid) {
                //Debug.LogError("OnEntityCreated not a Enemy" + pEntity->EnumType());
                return;
            }

            var pBoid = (Boid*) pEntity;
            if (_entityPrefabs == null) {
                //TODO read config to setup Entity Prefabs  
                var _spawners = GameObject.FindObjectsOfType<SSSamples.Boids.Authoring.SpawnRandomInSphere>();
                _entityManager = Unity.Entities.World.Active.EntityManager;
                _entityPrefabs = new Unity.Entities.Entity[_spawners.Length];
                for (int i = 0; i < _spawners.Length; i++) {
                    _entityPrefabs[i] = _spawners[i].PrefabEntity;
                }
            }

            var uEntity = _entityManager.Instantiate(_entityPrefabs[pBoid->AssetData.AssetId]);
            _id2GameObject[pEntity->LocalId] = uEntity;
            _entityManager.SetComponentData(uEntity, new Unity.Transforms.LocalToWorld {
                Value = float4x4.TRS(
                    pBoid->LocalToWorld.Position.ToVector3(),
                    //quaternion.identity,
                    quaternion.LookRotationSafe(pBoid->LocalToWorld.Forward.ToVector3(), Unity.Mathematics.math.up()),
                    new float3(1.0f, 1.0f, 1.0f))
            });
            _entityManager.AddComponentData(uEntity, pEntity->_ref);
        }

        public override void OnEntityDestroy(Context f, Lockstep.UnsafeECS.Entity* entity){
            if (_id2GameObject.TryGetValue(entity->LocalId, out var uEntity)) {
                _entityManager.DestroyEntity(uEntity);
                _id2GameObject.Remove(entity->LocalId);
            }
        }
    }
}