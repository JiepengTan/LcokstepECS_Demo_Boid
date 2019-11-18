using System.Linq;
using Lockstep.Serialization;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using Lockstep.InternalUnsafeECS;
using System.Collections;
using Lockstep.Math;
using System.Collections.Generic;
using Lockstep.Game;
using Lockstep.Logging;
using Lockstep.Util;
using UnityEngine;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class EntityViewBoidObstacle : BaseEntityView {
        private GameObject skillGo;
        static Dictionary<int,EntityViewBoidObstacle> _playerId2View = new Dictionary<int, EntityViewBoidObstacle>();

        public static EntityViewBoidObstacle GetView(int playerId){
            if (_playerId2View.TryGetValue(playerId, out var view))
                return view;
            return null;
        }
        public override void OnBindEntity(){
            _playerId2View[_cloneEntity.Player.LocalId] = this;
        }
        public override void OnUnbindEntity(){
            _playerId2View.Remove(_cloneEntity.Player.LocalId);
        }
        public override void OnSkillFire(LFloat range){
            var tarns = transform;
            if (skillGo == null) {
                var go = GameObject.Instantiate(UnityEntityService.ViewConfig.skillEffectPrefab, tarns, false);
                go.transform.SetParent(tarns, false);
                skillGo = go;
            }

            skillGo.SetActive(true);
            skillGo.transform.localScale = Vector3.one * (range.ToFloat() * 2);
        }

        public override void OnSkillDone(LFloat range){
            skillGo?.SetActive(false);
        }
    }
}