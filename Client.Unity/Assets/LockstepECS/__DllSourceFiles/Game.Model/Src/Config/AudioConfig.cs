using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Lockstep.Game {
    public partial class AudioConfig  {
        const ushort AudioIdAdd = 8001;
        const ushort AudioIdDead = 8002;
        const ushort AudioIdHitBrick = 8003;
        const ushort AudioIdHitIron = 8004;
        const ushort AudioIdStart = 8005;
        
        public ushort BgMusic;
        public ushort StartMusic;// = AudioIdStart;

    }
}