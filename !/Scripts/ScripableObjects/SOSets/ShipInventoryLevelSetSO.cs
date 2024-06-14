using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Inventory Level Set")]
public class ShipInventoryLevelSetSO : ScriptableObject
{
    public List<ShipInventoryLevelSO> shipInventoryLevelSOList;
}
