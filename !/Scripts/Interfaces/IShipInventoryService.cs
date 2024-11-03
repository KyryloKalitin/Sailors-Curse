using System;

public interface IShipInventoryService
{
    public event Action OnStatChanged;

    public int WaterAmount { get; }
    public int FoodAmount { get; }
    public int MaterialsAmount { get; }
}
