using System.Collections.Generic;


[System.Serializable]
public struct LastGameEvents
{
    public List<StatGameEventSOData> StatGameEventSOList;
    public List<LootGameEventSOData> LootGameEventSOList;

    public List<GameEventSO> GameEventsSOList
    {
        get
        {
            List<GameEventSO> allEventsList = new List<GameEventSO>();

            if (LootGameEventSOList != null)
            {
                foreach (var gameEventData in LootGameEventSOList)
                {
                    var gameEvent = GameEventSOFactory.CreateGameEventSO<LootGameEventSO>(gameEventData);
                    allEventsList.Add(gameEvent);
                }
            }

            if (StatGameEventSOList != null)
            {
                foreach (var gameEventData in StatGameEventSOList)
                {
                    var gameEvent = GameEventSOFactory.CreateGameEventSO<StatGameEventSO>(gameEventData);
                    allEventsList.Add(gameEvent);
                }
            }

            return allEventsList;
        }
    }

    public LastGameEvents(List<GameEventSO> gameEventSOList)
    {
        StatGameEventSOList = new();
        LootGameEventSOList = new();

        if (gameEventSOList.Count == 0)
            return;

        foreach (var gameEvent in gameEventSOList)
        {
            if(gameEvent is StatGameEventSO statGameEventSO)
            {
                StatGameEventSOList.Add(new StatGameEventSOData(statGameEventSO));
            }
            else if(gameEvent is LootGameEventSO lootGameEventSO)
            {
                LootGameEventSOList.Add(new LootGameEventSOData(lootGameEventSO));
            }
        }
    }
}

[System.Serializable]
public struct StatGameEventSOData
{
    public List<StatsType_Saturation> statsChanged;
    public string description;

    public StatGameEventSOData(StatGameEventSO statGameEventSO)
    {
        statsChanged = statGameEventSO.statsChanged;
        description = statGameEventSO.description;
    }
}

[System.Serializable]
public struct LootGameEventSOData
{
    public List<Item_Amount> inventoryItemsSOList;
    public string description;

    public LootGameEventSOData(LootGameEventSO lootGameEventSO)
    {
        inventoryItemsSOList = lootGameEventSO.inventoryItemsSOList;
        description = lootGameEventSO.description;
    }
}
