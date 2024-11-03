using System;
using System.Collections.Generic;


public class ShipInventoryService : ShipZoneInventoryService
{
    public event Action<StatsType> OnStatOver;
    public event Action OnRareItemsListChanged;

    public ShipInventoryLevelSO CurrentShipInventoryLevelSO { get; private set; }

    public ShipInventoryService()
    {
        CurrentShipInventoryLevelSO = SOSetLoader.LoadShipInventoryLevelSOSet().shipInventoryLevelSOList[0];

        _statsList = new Dictionary<StatsType, StatValues>
        {
            { StatsType.Food,       new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.foodMaxAmount    } },
            { StatsType.Water,      new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.waterMaxAmount   } },
            { StatsType.Material,   new StatValues() { maxStatAmount = CurrentShipInventoryLevelSO.materialMaxAmount} }
        };
    }

    public void DeserializeFromData(ShipInventoryData data)
    {
        CurrentShipInventoryLevelSO = SOSetLoader.LoadShipInventoryLevelSOSet().FindByName(data.shipInventoryLevelSOName);

        // Load stat values from data
        var foodAmount = new StatsType_Saturation()     { statsType = StatsType.Food, statAmount = data.foodAmount };
        var waterAmount = new StatsType_Saturation()    { statsType = StatsType.Water, statAmount = data.waterAmount };
        var materialAmount = new StatsType_Saturation() { statsType = StatsType.Material, statAmount = data.materialAmount };

        UpdateStat(foodAmount);
        UpdateStat(waterAmount);
        UpdateStat(materialAmount);

        var rareItemSet = SOSetLoader.LoadRareItemSOSet();

        foreach (var rareItem in data.rareItemSONames)
        {
            var itemSO = rareItemSet.FindByName(rareItem);

            if (itemSO != null)
                UpdateRareItemsList(itemSO);
        }

        WeaponSO = SOSetLoader.LoadWeaponSOSet().FindByName(data.weaponSOName);
    }

    public override void UpdateStats(List<StatsType_Saturation> statsList)
    {
        if (statsList.Count == 0)
            return;

        foreach (var stat in statsList)
        {
            UpdateStat(stat);
        }
    }

    public override void UpdateStat(StatsType_Saturation stat)
    {
        StatsType statType = stat.statsType;
        int statAmount = stat.statAmount;

        _statsList[statType].currentStatAmount += statAmount;

        if (_statsList[statType].currentStatAmount > _statsList[statType].maxStatAmount)
            _statsList[statType].currentStatAmount = _statsList[statType].maxStatAmount;

        if (_statsList[statType].currentStatAmount < 0)
            OnStatOver?.Invoke(statType);

        OnStatChangedInvoke();
    }

    public override void UpdateRareItemsList(RareInventoryItemSO item)
    {
        if (RareItemsList.Count >= CurrentShipInventoryLevelSO.rareItemsMaxAmount)
            return;

        if (RareItemsList.Contains(item))
            return;

        RareItemsList.Add(item);

        OnRareItemsListChanged?.Invoke();
    }

    public void ApplyChangesAfterRound(ShipZoneInventoryService shipZoneInventoryService)
    {
        var foodAmount      = new StatsType_Saturation(StatsType.Food, shipZoneInventoryService.FoodAmount);
        var waterAmount     = new StatsType_Saturation(StatsType.Water, shipZoneInventoryService.WaterAmount);
        var materialAmount  = new StatsType_Saturation(StatsType.Material, shipZoneInventoryService.MaterialsAmount);

        UpdateStat(foodAmount);
        UpdateStat(waterAmount);
        UpdateStat(materialAmount);

        WeaponSO = shipZoneInventoryService.WeaponSO;

        foreach (var item in shipZoneInventoryService.RareItemsList)
        {
            UpdateRareItemsList(item);
        }        
    }
}
