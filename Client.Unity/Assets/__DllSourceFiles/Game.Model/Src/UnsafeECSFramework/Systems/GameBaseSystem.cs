using Lockstep.UnsafeECS;
namespace Lockstep.UnsafeECS.Game {
    public partial class GameBaseSystem :BaseSystem{
        protected Context _context => (Context) _baseContext;
    }
    
    public abstract unsafe class GameJobSystem : BaseJobSystem {
        protected Context _context => (Context) _baseContext;
    }
}