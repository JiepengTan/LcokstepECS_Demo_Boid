using Lockstep.Game;
using Lockstep.Math;
using Lockstep.UnsafeECS;
using Entity = Lockstep.UnsafeECS.Entity;
using IContext = Lockstep.UnsafeECS.IContext;
using IInitializeSystem = Lockstep.UnsafeECS.IInitializeSystem;

namespace Lockstep.UnsafeECS.Game {
    public unsafe class InitSystem : GameBaseSystem, IInitializeSystem {
        public void Initialize(IContext context){
            for (int i = 0; i < TestData.ItemCount; i++) {
                TestData.idx++;
                var pItem = _context.PostCmdCreateEnemy();
                pItem->Unit.Health = 10;
                pItem->Unit.Timer = new LFloat(TestData.LoopTime) * i / TestData.ItemCount;
                pItem->Transform2D.pos = new LVector2(TestData.idx / TestData.SqrtCount, TestData.idx % TestData.SqrtCount);
            }
        }
    }


    public unsafe partial class CreateDestroySystem : GameExecuteSystem {
        public void Execute(Entity* entity, ref UnitData unit, ref Transform2D transform2D){
            if (unit.Health == 0) {
                _context.PostCmdDestroyEntity(entity);
                TestData.idx++;
                var enemy = _context.PostCmdCreateEnemy();
                enemy->Unit.Health = 10;
                enemy->Unit.Timer = 0;
                enemy->Transform2D.pos = new LVector2(TestData.idx / TestData.SqrtCount % TestData.SqrtCount, TestData.idx % TestData.SqrtCount);
            }
        }
    }
    
    public unsafe partial class MoveSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public LFloat DeltaTime;
            public void Execute(ref Transform2D transform2D){
                transform2D.pos = transform2D.pos + DeltaTime * LVector2.left;
            }
        }
        
        protected override void BeforeSchedule(){
            JobData.DeltaTime = new LFloat(null,30);
        }
    }
    
    public unsafe partial class MoveForwardSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public LFloat DeltaTime;
            public void Execute(ref Transform2D transform2D){
                transform2D.pos = transform2D.pos + (DeltaTime * 4) * LVector2.up;
            }
        }
        
        protected override void BeforeSchedule(){
            JobData.DeltaTime = new LFloat(null,30);
        }
    }
    public unsafe partial class TimerSystem : GameJobSystem {
        public unsafe partial struct JobDefine {
            public void Execute(ref UnitData unit){
                unit.Timer += new LFloat(true,30000);
                if (unit.Timer >= 5) {
                    unit.Health = 0;
                }
            }
        }
    }
}