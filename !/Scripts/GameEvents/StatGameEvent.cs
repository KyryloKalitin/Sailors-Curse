public class StatGameEvent : GameEvent
{
    public StatGameEvent(StatGameEventSO statGameEventSO) : base(statGameEventSO) { }

    public override void ApplyEventToInventory(ShipInventoryService shipInventoryService)
    {
        StatGameEventSO statGameEventSO = (StatGameEventSO)eventSO;

        shipInventoryService.UpdateStats(statGameEventSO.statsChanged);
    }
}



