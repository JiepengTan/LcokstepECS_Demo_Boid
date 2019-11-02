using System;
using System.Runtime.CompilerServices;
using Lockstep.Game;
using Lockstep.Game.UI;
using Lockstep.UnsafeECS;
using Lockstep.UnsafeECS.Game;
using NetMsg.Common;
using UnityEngine;

public class MainScript : MonoBehaviour {
    public UnsafeWorld _world;
    ServiceContainer _serviceContainer ;

    void Awake(){_serviceContainer = new UnityServiceContainer();
        var ctx =_serviceContainer.GetService<IECSFactoryService>().CreateContexts();
        _world = new UnsafeWorld(_serviceContainer,ctx,
            _serviceContainer.GetService<IECSFactoryService>().CreateSystems(ctx,_serviceContainer) );
    }

    private void Start(){
        _world.SimulationAwake(_serviceContainer,null);
        _world.SimulationStart(new Msg_G2C_GameStartInfo() {
            UserInfos = new GameData[1]{ new GameData()}
        }, 0);
    }

    private void Update(){
        _world.Step();
    }

    private void OnDestroy(){
        _world.DoDestroy();
    }
}