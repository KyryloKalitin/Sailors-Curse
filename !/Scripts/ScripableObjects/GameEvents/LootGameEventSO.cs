using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/LootGameEvent")]
public class LootGameEventSO : GameEventSO
{
    public List<Item_Amount> inventoryItemsSOList;

    public override void InitializeFromData(object data)
    {
        if (data is LootGameEventSOData statEventData)
        {
            inventoryItemsSOList = statEventData.inventoryItemsSOList;
            description = statEventData.description;
        }
    }
}

