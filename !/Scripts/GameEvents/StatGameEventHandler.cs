public class StatGameEventHandler : IGameEventHandler
{
    private StatGameEventSO _eventSO;

    public StatGameEventHandler(StatGameEventSO eventSO)
    {
        _eventSO = eventSO;
    }

    public void ApplyToInventory(ShipInventoryService shipInventoryService)
    {
        shipInventoryService.UpdateStats(_eventSO.statsChanged);
    }
}



