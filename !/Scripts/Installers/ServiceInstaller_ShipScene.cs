using System;
using UnityEngine;
using Zenject;

public class ServiceInstaller_ShipScene : MonoInstaller
{
    public override void InstallBindings()
    {
        InputServiceBind();
        ShipInventoryServiceBind();
        GameEventManagerBind();
    }

    private void GameEventManagerBind()
    {
        Container.BindInterfacesAndSelfTo<GameEventManager>().AsSingle();
    }

    private void InputServiceBind()
    {
        Container.Bind<InputService>().FromNew().AsSingle();
    }

    private void ShipInventoryServiceBind()
    {
        Container.Bind<IShipInventoryService>().To<ShipInventoryService>().FromNew().AsSingle();
    }
}