using System.Collections.Generic;
using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.UnsafeECS.Game {
    public partial struct TargetMoveInfo {
        public LVector3 GetPosition(LFloat timer){
            var rad = (InitDeg + ((timer / Interval)- (timer / Interval).Floor()) * 360) * LMath.Deg2Rad;
            return InitPos + new LVector3(LMath.Cos(rad), 0, LMath.Sin(rad)) * Radius;
        }
        public LVector3 GetForward(LFloat timer){
            var rad = (InitDeg + ((timer / Interval)- (timer / Interval).Floor()) * 360) * LMath.Deg2Rad;
            return InitPos + new LVector3(LMath.Cos(rad), 0, LMath.Sin(rad)) * Radius;
        }
    }
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ConfigSpawnInfo {
        public int Count;
        public LFloat Radius;
        public LVector3 Position;
        public int AssetId;
    }
        
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ConfigObstacleInfo {
        public LVector3 Position;
        public LVector3 Forward;
        public LFloat Scale;
        public LFloat Deg;
        public int AssetId;
        public SkillData SkillData;
        public MoveData MoveData;
    }
    
            
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ConfigTargetInfo {
        public TargetMoveInfo MoveInfo;
        public int AssetId;
    }
    
    
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ConfigBoidSharedData {
        public LFloat CellRadius;
        public LFloat SeparationWeight;
        public LFloat AlignmentWeight;
        public LFloat TargetWeight;
        public LFloat ObstacleAversionDistance;
        public LFloat MoveSpeed;
        //game logic
        public LFloat SinkTime;
        public LFloat SinkSpd;
    }
}