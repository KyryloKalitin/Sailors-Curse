using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct GameProgressData
{
    public ShipInventoryData shipInventoryData;
    public PlayerStatsData playerStatsData;
    public LastGameEvents lastGameEvents;

    public int daysAmount;

    public static GameProgressData CreateDefault()
    {
        ShipInventoryData defaultShipInventoryData = new ShipInventoryData(new ShipInventoryService());
        PlayerStatsData defaultPlayerStatsData = new PlayerStatsData(new PlayerStatsService());

        int defaultDaysAmount = 0;


        return new GameProgressData()
        {
            shipInventoryData = defaultShipInventoryData,
            playerStatsData = defaultPlayerStatsData,
            daysAmount = defaultDaysAmount,
            lastGameEvents = new()
        };
    }

    public GameProgressData(ShipInventoryService shipInventoryService, PlayerStatsService playerStatsService) 
    {
        shipInventoryData = new ShipInventoryData(shipInventoryService);
        playerStatsData = new PlayerStatsData(playerStatsService);

        daysAmount = GameProgressDataIO.LoadData().daysAmount;

        lastGameEvents = new();
    }

    public GameProgressData(ShipInventoryService shipInventoryService, PlayerStatsService playerStatsService, int daysToAdd, List<GameEventSO> gameEventSOList)
        : this(shipInventoryService, playerStatsService)
    {
        daysAmount += daysToAdd;

        lastGameEvents = new(gameEventSOList);
    }

    [System.Serializable]
    public struct ShipInventoryData
    {
        public int foodAmount;
        public int waterAmount;
        public int materialAmount;

        public List<RareInventoryItemSO> rareItemsList;

        public WeaponSO weaponSO;

        public ShipInventoryLevelSO shipInventoryLevel;

        public ShipInventoryData(ShipInventoryService shipInventoryService)
        {
            foodAmount = shipInventoryService.AmountFood;
            waterAmount = shipInventoryService.AmountWater;
            materialAmount = shipInventoryService.AmountMaterials;

            rareItemsList = shipInventoryService.RareItemsList;

            if (shipInventoryService.Weapon != null)
                weaponSO = (WeaponSO)shipInventoryService.Weapon.ItemSO;
            else
                weaponSO = null;

            shipInventoryLevel = shipInventoryService.CurrentShipInventoryLevelSO;
        }
    }

    [System.Serializable]
    public struct PlayerStatsData
    {
        public float HP;
        public List<TypedHit> typedHitsList;
        public PlayerStatsData(PlayerStatsService playerStatsService)
        {
            HP = playerStatsService.HP;
            typedHitsList = playerStatsService.TypedHitsList;
        }
    }

    [System.Serializable]
    public struct LastGameEvents
    {
        public List<StatGameEventSO> statGameEventSOList;
        public List<LootGameEventSO> lootGameEventSOList;

        public LastGameEvents(List<GameEventSO> gameEventSOList)
        {
            statGameEventSOList = gameEventSOList.OfType<StatGameEventSO>().ToList();
            lootGameEventSOList = gameEventSOList.OfType<LootGameEventSO>().ToList();
        }

        public List<GameEventSO> GetGameEventsList()
        {
            List<GameEventSO> allEventsList = new List<GameEventSO>();

            if (lootGameEventSOList != null)
                allEventsList.AddRange(lootGameEventSOList);

            if (statGameEventSOList != null)
                allEventsList.AddRange(statGameEventSOList);

            return allEventsList;
        }

    }
}
