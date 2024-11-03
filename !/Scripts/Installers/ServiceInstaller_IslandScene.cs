using Zenject;

public class ServiceInstaller_IslandScene : MonoInstaller
{
    public override void InstallBindings()
    {
        InputServiceBind();
        SceneLoaderBind();
        GameTimerServiceBind();
        ShipZoneInventoryServiceBind();
        PlayerInventoryServiceBind();
        PlayerStatsServiceBind();
    }

    private void GameTimerServiceBind()
    {
        Container.Bind<GameTimerService>().FromNew().AsSingle();
    }

    private void PlayerStatsServiceBind()
    {
        Container.Bind<PlayerStatsService>().FromNew().AsSingle();
    }

    private void InputServiceBind()
    {
        Container.Bind<InputService>().FromNew().AsSingle();
    }

    private void ShipZoneInventoryServiceBind()
    {
        Container.Bind<IShipInventoryService>().To<ShipZoneInventoryService>().FromNew().AsSingle();
    }

    private void PlayerInventoryServiceBind()
    {
        Container.Bind<PlayerInventoryService>().FromNew().AsSingle();
    }

    private void SceneLoaderBind()
    {
        var sceneLoader = Container.InstantiateComponentOnNewGameObject<SceneLoadService>("SceneLoader");

        Container
            .Bind<SceneLoadService>()
            .FromInstance(sceneLoader)
            .AsSingle()
            .NonLazy();
    }
}

