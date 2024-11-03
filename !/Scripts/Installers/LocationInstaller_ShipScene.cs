using UnityEngine;
using Zenject;

public class LocationInstaller_ShipScene : MonoInstaller
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    public override void InstallBindings()
    {
        PlayerBind();
        SceneLoaderBind();
    }

    private void PlayerBind()
    {
        ShipPlayerController playerController = Container
            .InstantiatePrefabForComponent<ShipPlayerController>(_playerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation, null);

        Container
            .Bind<PlayerController>()
            .FromInstance(playerController)
            .AsSingle()
            .NonLazy();
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
