using Lockstep.UnsafeECS;
namespace Lockstep.UnsafeECS.Game {
    public partial class GameExecuteSystem :BaseExecuteSystem{
        protected Context _context => (Context) _baseContext;
    }
}