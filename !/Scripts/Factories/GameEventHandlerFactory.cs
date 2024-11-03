using System;

public static class GameEventHandlerFactory
{
    public static IGameEventHandler CreateHandler(GameEventSO eventSO)
    {
        switch(eventSO)
        {
            case DamageGameEventSO gameEventSO:
                return new DamageGameEventHandler(gameEventSO);
            case StatGameEventSO statEventSO:
                return new StatGameEventHandler(statEventSO);
            case LootGameEventSO lootEventSO:
                return new LootGameEventHandler(lootEventSO);
            default:
                throw new NotSupportedException("Unknown GameEventSO type");                
        }
    }

    public static IConditionalGameEventHandler CreateHandler(ItemGameEventSO eventSO)
    {
        switch (eventSO)
        {
            case ComplexItemGameEventSO complexItemEventSO:
                return new ComplexItemGameEventHandler(complexItemEventSO);
            case ItemGameEventSO itemEventSO:
                return new ItemGameEventHandler(itemEventSO);
            default:
                throw new NotSupportedException("Unknown GameEventSO type");
        }
    }
}



