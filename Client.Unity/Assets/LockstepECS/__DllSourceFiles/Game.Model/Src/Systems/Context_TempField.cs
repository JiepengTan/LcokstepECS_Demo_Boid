using System.Collections.Generic;
using System.Runtime.InteropServices;
using Lockstep.Game;
using Lockstep.Logging;
using Lockstep.Math;
using NetMsg.Common;
using Unity.Collections;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class TempFields {
        private NativeArray<LVector3> _AllTargetPos=>_context.GetAllTransform3D_Position(EAllocatorType.System, E_EntityOfTransform3D.BoidTarget);
        private NativeArray<LVector3> _AllObstaclePos=>_context.GetAllTransform3D_Position(EAllocatorType.System, E_EntityOfTransform3D.BoidObstacle);
        private NativeArray<CellData> _AllCellData;
        private NativeMultiHashMap<int, int> _HashMap;
        private bool _HasInitMap = false;
        
        public NativeArray<LVector3> AllTargetPos => _AllTargetPos;
        public NativeArray<LVector3> AllObstaclePos => _AllObstaclePos;

        public NativeArray<CellData> AllCellData {
            get {
                if (_AllCellData.Length != _context.CurBoidCount) {
                    if (_AllCellData.Length != 0) {
                        _AllCellData.Dispose();
                    }

                    _AllCellData = new NativeArray<CellData>(_context.CurBoidCount,
                        Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                }

                return _AllCellData;
            }
        }

        public NativeMultiHashMap<int, int> GetHashMap(){
            return _HashMap;
        }

        public NativeMultiHashMap<int, int> CreateHashMap(){
            if (_HasInitMap) {
                _HashMap.Dispose();
            }
            _HasInitMap = true;
            _HashMap = new NativeMultiHashMap<int, int>(_context.CurBoidCount,
                Unity.Collections.Allocator.Persistent);
            return _HashMap;
        }

        public void OnDestroy(){
            if (_AllCellData.Length != 0) {
                _AllCellData.Dispose();
            }

            if (_HasInitMap) {
                _HashMap.Dispose();
                _HasInitMap = false;
            }
            Clean();
        }
        public void FramePrepare(){}

        public void FrameClearUp(){
            Clean();
        }
        private void Clean(){}
    }
}