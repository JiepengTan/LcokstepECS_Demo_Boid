using Lockstep.Game;
using Lockstep.UnsafeECS.Game;


public class UnityServiceContainer : BaseGameServicesContainer {
    public UnityServiceContainer():base(){
        RegisterService(new UnsafeEcsFactoryService());
        RegisterService(new UnityEntityService());
    }
}