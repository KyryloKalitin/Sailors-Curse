using System.Collections.Generic;
using UnityEngine;

public class GameEventSOSet : ScriptableObject
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

    public GameEventSO FindEventByName(string name)
    {
        if (FindInList(foodStartingEvents, name)        is GameEventSO foodEvent) return foodEvent;
        if (FindInList(waterStartingEvents, name)       is GameEventSO waterEvent) return waterEvent;
        if (FindInList(materialsStartingEvents, name)   is GameEventSO materialsEvent) return materialsEvent;
        if (FindInList(rareItemsStartingEvents, name)   is GameEventSO rareItemsEvent) return rareItemsEvent;
        if (FindInList(statEvents, name)                is GameEventSO statEvent) return statEvent;
        if (FindInList(itemEvents, name)                is GameEventSO itemEvent) return itemEvent;
        if (FindInList(damageGameEvents, name)          is GameEventSO damageEvent) return damageEvent;

        if (defaultStatLossSO.name == name && defaultStatLossSO is GameEventSO defaultLossEvent) return defaultLossEvent;

        return null; 
    }

    private GameEventSO FindInList<T>(List<Item_Chance<T>> eventList, string name) where T : GameEventSO
    {
        foreach (var gameEvent in eventList)
        {
            if (gameEvent.itemSO.name == name)
            {
                return gameEvent.itemSO;
            }
        }

        return null;
    }

    private GameEventSO FindInList(List<DamageGameEventSO> eventList, string name)
    {
        foreach (var gameEvent in eventList)
        {
            if (gameEvent.name == name)
            {
                return gameEvent;
            }
        }

        return null;
    }
}




