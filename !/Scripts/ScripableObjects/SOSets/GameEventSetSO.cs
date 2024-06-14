using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSetSO : ScriptableObject
{
    [Header("Starting Game Events")]

    public List<Item_Chance<LootGameEventSO>> foodStartingEvents;
    public List<Item_Chance<LootGameEventSO>> waterStartingEvents;
    public List<Item_Chance<LootGameEventSO>> materialsStartingEvents;

    public List<Item_Chance<LootGameEventSO>> rareItemsStartingEvents;

    [Header("Stat Game Events")]

    public List<Item_Chance<StatGameEventSO>> statEvents;

    [Header("Item Game Events")]

    public List<Item_Chance<ItemGameEventSO>> itemEvents;

    [Header("Damage Game Events")]

    public List<DamageGameEventSO> damageGameEvents;

    [Header("Default Resources Loss")]

    public StatGameEventSO defaultStatLossSO;
}




