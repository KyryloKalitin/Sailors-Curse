using UnityEngine;
using Zenject;

public class LocationInstaller_IslandScene : MonoInstaller
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Transform _shipZoneSpawnPoint;
    [SerializeField] private GameObject _shipZonePrefab;

    public override void InstallBindings()
    {
        ShipZoneBind();
        GameManagerBind();
        PlayerBind();
    }

    private void PlayerBind()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();

        IslandPlayerController playerController = Container
            .InstantiatePrefabForComponent<IslandPlayerController>(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, null);

        if (gameProgressData.shipInventoryData.weaponSO != null)
        { 
            Weapon weapon = Instantiate(gameProgressData.shipInventoryData.weaponSO.prefab.gameObject).GetComponent<Weapon>();
            playerController.GetPlayerInventoryService().SetWeapon(weapon);        
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

}
