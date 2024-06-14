using UnityEngine;
using Zenject;

public class ServiceInstaller_IslandScene : MonoInstaller
{
    public override void InstallBindings()
    {
        InputServiceBind();

        ShipInventoryServiceBind();
        PlayerInventoryServiceBind();
        PlayerStatsServiceBind();
    }

    private void PlayerStatsServiceBind()
    {
        Container.Bind<PlayerStatsService>().FromNew().AsSingle();
    }

    private void InputServiceBind()
    {
        Container.Bind<InputService>().FromNew().AsSingle();
    }

    public void ShipInventoryServiceBind()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();        

        ShipInventoryService shipInventoryService = new(gameProgressData.shipInventoryData.shipInventoryLevel);

        Container.Bind<ShipInventoryService>().FromInstance(shipInventoryService).AsSingle();
    }

    public void PlayerInventoryServiceBind()
    {
        Container.Bind<PlayerInventoryService>().FromNew().AsSingle();
    }
}

