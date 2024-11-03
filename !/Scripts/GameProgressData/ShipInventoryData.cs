using System.Collections.Generic;


[System.Serializable]
public struct ShipInventoryData
{
    public int foodAmount;
    public int waterAmount;
    public int materialAmount;

    public List<string> rareItemSONames;

    public string weaponSOName;

    public string shipInventoryLevelSOName;

    public ShipInventoryData(ShipInventoryService shipInventoryService)
    {
        foodAmount = shipInventoryService.FoodAmount;
        waterAmount = shipInventoryService.WaterAmount;
        materialAmount = shipInventoryService.MaterialsAmount;

        rareItemSONames = new();

        foreach (var item in shipInventoryService.RareItemsList)
        {
            rareItemSONames.Add(item.name);
        }

        if (shipInventoryService.WeaponSO != null)
            weaponSOName = shipInventoryService.WeaponSO.name;
        else
            weaponSOName = null;

        shipInventoryLevelSOName = shipInventoryService.CurrentShipInventoryLevelSO.name;
    }
}

