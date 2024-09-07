using System;
using UnityEngine;

public class ComplexItemGameEventHandler : ItemGameEventHandler
{
    private ComplexItemGameEventSO _eventSO;

    public ComplexItemGameEventHandler(ComplexItemGameEventSO eventSO) : base(eventSO)
    {
        _eventSO = eventSO;
    }

    public override bool TryApplyToInventory(ShipInventoryService shipInventoryService)
    {
        if (shipInventoryService.RareItemsList.Contains(_eventSO.rareInventoryItemSO))
        {
            return true;
        }
        else
        {
            var alternativeHandler = GameEventHandlerFactory.CreateHandler(_eventSO.alternativeItemGameEventSO);
            alternativeHandler.ApplyToInventory(shipInventoryService);

            return false;
        }
    }
}

public static class GameEventHandlerFactory
{
    public static IGameEventHandler CreateHandler(GameEventSO eventSO)
    {
        switch(eventSO)
        {
            case ComplexItemGameEventSO complexItemEventSO:
                return new ComplexItemGameEventHandler(complexItemEventSO);
            case ItemGameEventSO itemEventSO:
                return new ItemGameEventHandler(itemEventSO);
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
}

public static class GameEventSOFactory
{
    public static T CreateGameEventSO<T>(object data) where T: GameEventSO
    {
        T gameEventSO = ScriptableObject.CreateInstance<T>();
        gameEventSO.InitializeFromData(data);

        return gameEventSO;
    }
}



