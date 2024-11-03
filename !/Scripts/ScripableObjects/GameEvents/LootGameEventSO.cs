using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/LootGameEvent")]
public class LootGameEventSO : GameEventSO
{
    public List<Item_Amount> inventoryItemsSOList;
}

