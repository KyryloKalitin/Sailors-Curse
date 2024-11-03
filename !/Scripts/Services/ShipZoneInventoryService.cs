using System;
using System.Collections.Generic;

public class ShipZoneInventoryService : IShipInventoryService
{
    public event Action OnStatChanged;

    public int WaterAmount
    {
        get => _statsList[StatsType.Water].currentStatAmount;
    }
    public int FoodAmount
    {
        get => _statsList[StatsType.Food].currentStatAmount;
    }
    public int MaterialsAmount
    {
        get => _statsList[StatsType.Material].currentStatAmount;
    }

    public List<RareInventoryItemSO> RareItemsList { get; protected set; } = new();
    public WeaponSO WeaponSO { get; set; }

    protected Dictionary<StatsType, StatValues> _statsList;

    public ShipZoneInventoryService()
    {
        _statsList = new Dictionary<StatsType, StatValues>
        {
            { StatsType.Food,       new StatValues() { maxStatAmount = int.MaxValue } },
            { StatsType.Water,      new StatValues() { maxStatAmount = int.MaxValue } },
            { StatsType.Material,   new StatValues() { maxStatAmount = int.MaxValue } }
        };
    }

    public virtual void UpdateStats(List<StatsType_Saturation> statsList)
    {
        if (statsList.Count == 0)
            return;

        foreach (var stat in statsList)
        {
            UpdateStat(stat);
        }
    }

    public virtual void UpdateStat(StatsType_Saturation stat)
    {
        StatsType statType = stat.statsType;
        int statAmount = stat.statAmount;

        _statsList[statType].currentStatAmount += statAmount;

        OnStatChangedInvoke();
    }

    public virtual void UpdateRareItemsList(RareInventoryItemSO item)
    {
        if (RareItemsList.Contains(item))
            return;

        RareItemsList.Add(item);
    }

    protected void OnStatChangedInvoke()
    {
        OnStatChanged?.Invoke();
    }

    protected class StatValues
    {
        public int currentStatAmount;
        public int maxStatAmount;
    }
}
