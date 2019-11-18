using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Lockstep.Math;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class CollisionSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            [ReadOnly] public NativeArray<Transform3D> obstaclePositions;
            [ReadOnly] public NativeArray<EntityRef> obstacleIdxs;
            [ReadOnly] public LFloat MinSqrMagnitude;
            [ReadOnly] public LFloat SinkTime;

            public void Execute(ref Transform3D transform3D, ref BoidState boidState){
                if (boidState.IsDied) return;
                var len = obstaclePositions.m_Length;
                if (len == 0) return;
                var ptr = obstaclePositions.GetPointer(0);
                for (int i = 0; i < len; ++i, ++ptr) {
                    var dist = (transform3D.Position - ptr->Position).sqrMagnitude;
                    var sqrScale = ptr->Scale * ptr->Scale;
                    if (dist < MinSqrMagnitude * sqrScale) {
                        boidState.IsDied = true;
                        boidState.IsScored = false;
                        boidState.SinkTimer = SinkTime;
                        boidState.Killer = obstacleIdxs[i];
                    }
                }
            }
        }

        protected override bool BeforeSchedule(){
            if (_gameConfigService.ObstacleInfos.Count <= 0) return false;
            //assign jobData info
            JobData.SinkTime = _gameConfigService.BoidSettting.SinkTime;
            var playser = _gameConfigService.ObstacleInfos[0];
            var rawVal = playser.SkillData.AtkRange;
            JobData.MinSqrMagnitude = rawVal * rawVal;

            NativeArray<Transform3D> compAry;
            NativeArray<EntityRef> entityAry;
            _context.GetAllBoidObstacle_Transform(EAllocatorType.System,out entityAry, out compAry,
                (ptr) => ptr->Skill.IsFiring);
            JobData.obstaclePositions = compAry;
            JobData.obstacleIdxs = entityAry;
            return true;
        }
    }
}