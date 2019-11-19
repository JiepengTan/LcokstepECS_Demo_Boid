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
    public unsafe partial class UnityEntityService : BaseUnityEntityService,IGameEventService {
      
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
    }
}