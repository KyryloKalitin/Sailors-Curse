using System.Collections.Generic;


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
        foodAmount = shipInventoryService.FoodAmount;
        waterAmount = shipInventoryService.WaterAmount;
        materialAmount = shipInventoryService.MaterialsAmount;

        rareItemsList = shipInventoryService.RareItemsList;

        if (shipInventoryService.Weapon != null)
            weaponSO = (WeaponSO)shipInventoryService.Weapon.ItemSO;
        else
            weaponSO = null;

        shipInventoryLevel = shipInventoryService.CurrentShipInventoryLevelSO;
    }
}

