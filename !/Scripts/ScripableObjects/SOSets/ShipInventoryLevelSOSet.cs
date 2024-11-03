using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOSet/ShipInventoryLevel")]
public class ShipInventoryLevelSOSet : ScriptableObject
{
    public List<ShipInventoryLevelSO> shipInventoryLevelSOList = new();

    public ShipInventoryLevelSO FindByName(string name)
    {
        foreach (var shipInventoryLevelSO in shipInventoryLevelSOList)
        {
            if (shipInventoryLevelSO.name == name)
                return shipInventoryLevelSO;
        }

        return null;
    }
}



