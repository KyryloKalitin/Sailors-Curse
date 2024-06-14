using System;
using UnityEngine;
using Zenject;

public class ServiceInstaller_ShipScene : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputService>().FromNew().AsSingle();
        ShipInventoryServiceBind();
        Container.Bind<GameEventManager>().FromNew().AsSingle();
    }

    private void ShipInventoryServiceBind()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();

        ShipInventoryService shipInventoryService = new(gameProgressData.shipInventoryData);

        Container.Bind<ShipInventoryService>().FromInstance(shipInventoryService).AsSingle();
    }
}