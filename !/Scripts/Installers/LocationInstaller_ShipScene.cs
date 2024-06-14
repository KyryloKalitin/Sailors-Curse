using UnityEngine;
using Zenject;

public class LocationInstaller_ShipScene : MonoInstaller
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    public override void InstallBindings()
    {
        PlayerBind();
    }

    private void PlayerBind()
    {
        ShipPlayerController playerController = Container
            .InstantiatePrefabForComponent<ShipPlayerController>(_playerPrefab, _playerSpawnPoint.position, Quaternion.Euler(new Vector3(0f,-90f,0f)), null);

        Container
            .Bind<PlayerController>()
            .FromInstance(playerController)
            .AsSingle()
            .NonLazy();
    }
}
