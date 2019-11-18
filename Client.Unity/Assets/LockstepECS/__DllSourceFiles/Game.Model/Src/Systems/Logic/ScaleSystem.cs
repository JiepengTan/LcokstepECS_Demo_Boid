using Lockstep.Math;
using Unity.Transforms;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class ScaleSystem : GameExecuteSystem {
        public void Execute(ref Transform3D transform3D, ref PlayerData playerData){
            _gameStateService.CurScale = _gameConfigService.InitScale * (playerData.Score.ToLFloat() / 300 + 1) ;
            transform3D.Scale = LMath.Max(LFloat.one,_gameStateService.CurScale) ;
        }
    }
}