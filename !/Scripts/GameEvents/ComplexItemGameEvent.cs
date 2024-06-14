public class ComplexItemGameEvent : ItemGameEvent
{
    public ComplexItemGameEvent(ComplexItemGameEventSO complexItemGameEventSO) : base(complexItemGameEventSO) { }

    public override bool TryApplyEventToInventory(ShipInventoryService shipInventoryService)
    {
        ComplexItemGameEventSO complexItemGameEventSO = (ComplexItemGameEventSO)eventSO;

        if (!base.TryApplyEventToInventory(shipInventoryService))
        {
            shipInventoryService.UpdateStats(complexItemGameEventSO.alternativeItemGameEventSO.statsChanged);
            return false;
        }
        else
            return true;
    }
}



