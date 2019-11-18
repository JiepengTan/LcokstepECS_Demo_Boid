
using Lockstep.UnsafeECS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Lockstep.Game.UI {
    public class UIGameStatus : UIBaseWindow {
        private Transform TextMsg => GetRef<Transform>("TextMsg");
        private Transform TextLevel => GetRef<Transform>("TextLevel");
        private Transform TextEnemyCount => GetRef<Transform>("TextEnemyCount");
        private Transform TextScore1 => GetRef<Transform>("TextScore1");
        private Transform TextLife1 => GetRef<Transform>("TextLife1");
        private Transform TextScore2 => GetRef<Transform>("TextScore2");
        private Transform TextLife2 => GetRef<Transform>("TextLife2");
        private RawImage RawImg => GetRef<RawImage>("RawImg");

      
        void ShowText(Transform parent, string txt){
            if (parent == null) return;
            var text = parent.GetComponent<Text>();
            if (text != null) {
                text.text = txt;
            }
        }

        public  override void DoStart(){
            RawImg.texture = ((UnityUIService) (_uiService)).rt;
#if UNITY_EDITOR
            //OpenWindow(UIDefine.UIDebugInfo);
#endif
        }

        void Update(){
            //UpdateStatus();
        }

        void UpdateStatus(){
            if (!GameStateService.Instance.IsPlaying) {
                return;
            }

            ShowText(TextLevel, (GlobalStateService.Instance.CurLevel).ToString());
        }
    }
}