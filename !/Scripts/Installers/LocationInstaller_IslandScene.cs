using System;
using UnityEngine;
using Zenject;

public class LocationInstaller_IslandScene : MonoInstaller
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Transform _shipZoneSpawnPoint;
    [SerializeField] private GameObject _shipZonePrefab;

    [SerializeField] private LoadingScreenController _loadingScreen;

    public override void InstallBindings()
    {
        ShipZoneBind();
        GameWinHandlerBind();
        GameLoseHandlerBind();
        LoadingScreenBind();
        GameManagerBind();
        PlayerBind();
    }

    private void GameWinHandlerBind()
    {
        Container.Bind<GameWinHandler>().FromNew().AsSingle();
    }

    private void GameLoseHandlerBind()
    {
        Container.Bind<GameLoseHandler>().FromNew().AsSingle();
    }

    private void PlayerBind()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();

        IslandPlayerController playerController = Container
            .InstantiatePrefabForComponent<IslandPlayerController>(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, null);

        if (!string.IsNullOrEmpty(gameProgressData.ShipInventoryData.weaponSOName))
        {
            WeaponSO weaponSO = SOSetLoader.LoadWeaponSOSet().FindByName(gameProgressData.ShipInventoryData.weaponSOName);

            Weapon weapon = Instantiate(weaponSO.prefab.gameObject).GetComponent<Weapon>();
            playerController.PlayerInventoryService.SetWeapon(weapon);        
        }    

        Container
            .Bind<PlayerController>()
            .FromInstance(playerController)
            .AsSingle()
            .NonLazy();
    }

    private void ShipZoneBind()
    {
        ShipZoneTriggerHandler shipZoneTriggerHandler = Container
            .InstantiatePrefabForComponent<ShipZoneTriggerHandler>(_shipZonePrefab, _shipZoneSpawnPoint.position, Quaternion.identity, null);

        Container
            .Bind<ShipZoneTriggerHandler>()
            .FromInstance(shipZoneTriggerHandler)
            .AsSingle()
            .NonLazy();
    }

    private void GameManagerBind()
    {
        IslandGameManager islandGameManager = Container.InstantiateComponentOnNewGameObject<IslandGameManager>("GameManager");

        Container
            .Bind<IslandGameManager>()
            .FromInstance(islandGameManager)
            .AsSingle()
            .NonLazy();
    }

    private void LoadingScreenBind()
    {
        Container
            .Bind<LoadingScreenController>()
            .FromInstance(_loadingScreen)
            .AsSingle()
            .NonLazy();
    }
}
