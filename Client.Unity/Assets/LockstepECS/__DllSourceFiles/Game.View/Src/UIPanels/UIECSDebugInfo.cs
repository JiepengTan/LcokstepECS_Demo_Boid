using Lockstep.UnsafeECS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Lockstep.Game.UI {
    public class UIECSDebugInfo : MonoBehaviour {
        public Text HashCodeText;
        public Text AgentCountText;

        public Text ConnectedText;
        public Text CurrentTickText;

        bool IsConnected => NetworkService.Instance?.IsConnected ?? false;
        //int CurTick => SimulationService.Instance?.World?.Tick ?? 0;

        void Update(){
            if (!GameStateService.Instance.IsPlaying) return;
            ConnectedText.text = $"IsConn: {IsConnected}";
            if (IsConnected) {
            }
        }
    }
}