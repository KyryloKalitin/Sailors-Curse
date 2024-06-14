using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventoryService
{
    public event Action OnStatOver;
    public event Action OnStatChanged;

    public int AmountWater
    {
        get => _statsList[StatsType.Water].currentStatAmount;

        private set => _statsList[StatsType.Water].currentStatAmount += value;    
    }
    public int AmountFood
    {
        get => _statsList[StatsType.Food].currentStatAmount;

        private set => _statsList[StatsType.Food].currentStatAmount += value;    
    }
    public int AmountMaterials
    {
        get => _statsList[StatsType.Material].currentStatAmount;

        private set => _statsList[StatsType.Material].currentStatAmount += value;
    }

    public ShipInventoryLevelSO CurrentShipInventoryLevelSO { get; private set; }
    public List<RareInventoryItemSO> RareItemsList { get; private set; }
    public Weapon Weapon { get; set; }

    private Dictionary<StatsType, StatValues> _statsList;

    private const string _SHIP_INVENTORY_LEVEL_PATH = "Data/ShipInventoryLevelSet";

    public static ShipInventoryService operator +(ShipInventoryService firstShipInventoryService, ShipInventoryService secondShipInventoryService)
    {
        ShipInventoryService result = new();

        List<StatsType_Saturation> summaryStatsList = new()
        {
            { new StatsType_Saturation() { statsType = StatsType.Food,      statAmount = firstShipInventoryService.AmountFood + secondShipInventoryService.AmountFood } },
            { new StatsType_Saturation() { statsType = StatsType.Water,     statAmount = firstShipInventoryService.AmountWater + secondShipInventoryService.AmountWater } },
            { new StatsType_Saturation() { statsType = StatsType.Material,  statAmount = firstShipInventoryService.AmountMaterials + secondShipInventoryService.AmountMaterials } }
        };

        result.UpdateStats(summaryStatsList);

        if(firstShipInventoryService.RareItemsList.Count != 0)
        {
            foreach (var item in firstShipInventoryService.RareItemsList)
            {
                result.UpdateRareItemsList(item.prefab.GetComponent<RareInventoryItem>());
            }
        }

        if(secondShipInventoryService.RareItemsList.Count != 0)
        {
            foreach (var item in secondShipInventoryService.RareItemsList)
            {
                result.UpdateRareItemsList(item.prefab.GetComponent<RareInventoryItem>());
            }
        }

        result.CurrentShipInventoryLevelSO =secondShipInventoryService.CurrentShipInventoryLevelSO != null ?
                                            secondShipInventoryService.CurrentShipInventoryLevelSO :
                                            firstShipInventoryService.CurrentShipInventoryLevelSO  != null ?
                                            firstShipInventoryService.CurrentShipInventoryLevelSO  : null;

        result.Weapon = secondShipInventoryService.Weapon != null ?
                        secondShipInventoryService.Weapon :
                        firstShipInventoryService.Weapon  != null ?
                        firstShipInventoryService.Weapon  : null;

        return result;
    }

    public ShipInventoryService()
    {
        CurrentShipInventoryLevelSO = Resources.Load<ShipInventoryLevelSetSO>(_SHIP_INVENTORY_LEVEL_PATH).shipInventoryLevelSOList[0];

        _statsList = new Dictionary<StatsType, StatValues>
        {
            { StatsType.Food,       new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.foodMaxAmount    } },
            { StatsType.Water,      new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.waterMaxAmount   } },
            { StatsType.Material,   new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.materialMaxAmount} }
        };

        RareItemsList = new List<RareInventoryItemSO>();

        Weapon = null;
    }

    public ShipInventoryService(ShipInventoryLevelSO shipInventoryLevelSO) : this()
    {
        CurrentShipInventoryLevelSO = shipInventoryLevelSO;

        _statsList = new Dictionary<StatsType, StatValues>
        {
            { StatsType.Food,       new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.foodMaxAmount    } },
            { StatsType.Water,      new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.waterMaxAmount   } },
            { StatsType.Material,   new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.materialMaxAmount} }
        };
    }

    public ShipInventoryService(GameProgressData.ShipInventoryData data) : this()
    {
        CurrentShipInventoryLevelSO = data.shipInventoryLevel;

        AmountFood = data.foodAmount;
        AmountWater = data.waterAmount;
        AmountMaterials = data.materialAmount;

        RareItemsList = data.rareItemsList;

        if(data.weaponSO != null)
            Weapon = data.weaponSO.prefab.GetComponent<Weapon>();
    }

    public void UpdateStats(List<StatsType_Saturation> statsList)
    {
        if (statsList.Count == 0)
            return;

        foreach (var stat in statsList)
        {
            StatsType statType = stat.statsType;
            int statAmount = stat.statAmount;

            _statsList[statType].currentStatAmount += statAmount;
            
            if(_statsList[statType].currentStatAmount > _statsList[statType].maxStatAmount)
                _statsList[statType].currentStatAmount = _statsList[statType].maxStatAmount;

            if (_statsList[statType].currentStatAmount < 0)
                OnStatOver?.Invoke();
        }

        OnStatChanged?.Invoke();
    }

    public void UpdateRareItemsList(RareInventoryItem item)
    {
        if (RareItemsList.Count >= CurrentShipInventoryLevelSO.rareItemsMaxAmount)
            return;

        if (RareItemsList.Contains((RareInventoryItemSO)item.ItemSO))
            return;

        RareItemsList.Add((RareInventoryItemSO)item.ItemSO);
    }

    public void GetStats()
    {
        Debug.Log("RareItemsList.Count(GetStats): " + RareItemsList.Count);

        Debug.Log("Inventory condition: " + "AmountFood: " + AmountFood + " " + "AmountWater" + AmountWater + " " + "AmountMaterials" + AmountMaterials);
    }

    private class StatValues
    {
        public int currentStatAmount;
        public int maxStatAmount;
    }
}
