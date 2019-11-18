using System.Collections.Generic;
using Lockstep.Math;
using Lockstep.UnsafeECS.Game;
using Lockstep.UnsafeECS;
using UnityEngine;
using Debug = Lockstep.Logging.Debug;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Entity = Lockstep.UnsafeECS.Entity;
using math = Lockstep.UnsafeECS.math;

namespace Lockstep.Game {
    public unsafe partial class UnityEntityService : BaseService, IEntityService, IGameEventService {
        private static Dictionary<int, Unity.Entities.Entity> _assetId2EntityPrefas =
            new Dictionary<int, Unity.Entities.Entity>();

        public static void RegisterUnityEntityPrefabs(List<EntityPrefabInfo> prefabs){
            _assetId2EntityPrefas.Clear();
            foreach (var prefab in prefabs) {
                if (_assetId2EntityPrefas.ContainsKey(prefab.AssetId)) {
                    Debug.LogError("Duplicate Prefab ID" + prefab.AssetId);
                }

                _assetId2EntityPrefas[prefab.AssetId] = prefab.Prefab;
            }

            Debug.Log("RegisterUnityEntityPrefabs ");
        }

        private Dictionary<int, Unity.Entities.Entity> _id2UnityEntity = new Dictionary<int, Unity.Entities.Entity>();
        private Dictionary<int, BaseEntityView> _id2EntityView = new Dictionary<int, BaseEntityView>();

        //private GameObject[] _prefabs;
        private Unity.Entities.EntityManager _entityManager;
        public static GameViewConfig ViewConfig;

        public override void DoAwake(IServiceContainer services){
            ViewConfig = Resources.Load<GameViewConfig>(GameViewConfig.ResPath);
            _entityManager = Unity.Entities.World.Active.GetOrCreateManager<EntityManager>();
        }

        public void OnSkillFire(Entity* ptr
            , ref SkillData skillData
        ){
            if (_id2EntityView.TryGetValue(ptr->LocalId, out var uView)) {
                uView.OnSkillFire(skillData.AtkRange);
            }

            Debug.Log("OnSkillFire");
        }

        public void OnSkillDone(Entity* ptr
            , ref SkillData skillData
        ){
            if (_id2EntityView.TryGetValue(ptr->LocalId, out var uView)) {
                uView.OnSkillDone(skillData.AtkRange);
            }
            Debug.Log("OnSkillDone");
        }

        public void OnEntityCreated(Context f, Lockstep.UnsafeECS.Entity* pEntity){
            if (pEntity == null) {
                Debug.LogError("OnEntityCreated null");
                return;
            }

            var pPrefab = EntityUtil.GetPrefab(pEntity);
            if (pPrefab == null) return;

            var assetId = pPrefab->AssetId;
            if (assetId == 0) return;
            Debug.Assert(_assetId2EntityPrefas.ContainsKey(assetId), "assetId" + assetId);
            var uEntity = _entityManager.Instantiate(_assetId2EntityPrefas[assetId]);
            _entityManager.AddComponentData(uEntity, pEntity->_ref);
            _id2UnityEntity[pEntity->LocalId] = uEntity;
            Transform3D* transform3D = EntityUtil.GetTransform3D(pEntity);
            if (transform3D != null) {
                _entityManager.SetComponentData(uEntity, new Unity.Transforms.LocalToWorld {
                    Value = float4x4.TRS(
                        transform3D->Position.ToVector3(),
                        quaternion.LookRotationSafe(transform3D->Forward.ToVector3(), Unity.Mathematics.math.up()),
                        new float3(1))
                });
            }

            if (!EntityViewUtil.HasView(pEntity)) {
                return;
            }

            //_entityManager.AddSharedComponentData(pEntity,new MeshRenderer());
            //bind view
            Debug.Assert(!_id2EntityView.ContainsKey(pEntity->_localId));
            var view = EntityViewUtil.BindEntityView(pEntity);
            view.BindEntity(pEntity);
            view.OnBindEntity();
            _id2EntityView[pEntity->_localId] = view;
        }

        public void OnEntityDestroy(Context f, Lockstep.UnsafeECS.Entity* pEntity){
            if (_id2EntityView.TryGetValue(pEntity->LocalId, out var uView)) {
                // ReSharper disable once Unity.NoNullPropogation
                uView?.OnUnbindEntity();
                uView?.UnbindEntity();
                _id2EntityView.Remove(pEntity->LocalId);
            }
            if (_id2UnityEntity.TryGetValue(pEntity->LocalId, out var uEntity)) {
                _entityManager.DestroyEntity(uEntity);
                _id2UnityEntity.Remove(pEntity->LocalId);
            }
        }


        public void OnBoidSpawnerCreated(Context context, BoidSpawner* entity){ }
        public void OnBoidSpawnerDestroy(Context context, BoidSpawner* entity){ }
        public void OnBoidCellCreated(Context context, BoidCell* entity){ }
        public void OnBoidCellDestroy(Context context, BoidCell* entity){ }
        public void OnBoidCreated(Context context, Boid* entity){ }
        public void OnBoidDestroy(Context context, Boid* entity){ }
        public void OnBoidTargetCreated(Context context, BoidTarget* entity){ }
        public void OnBoidTargetDestroy(Context context, BoidTarget* entity){ }
        public void OnBoidObstacleCreated(Context context, BoidObstacle* entity){ }
        public void OnBoidObstacleDestroy(Context context, BoidObstacle* entity){ }
    }
}