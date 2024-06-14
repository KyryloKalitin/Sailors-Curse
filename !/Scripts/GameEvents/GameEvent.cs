public abstract class GameEvent
{
    public GameEventSO eventSO;

    public GameEvent(GameEventSO eventSO)
    {
        this.eventSO = eventSO;
    }

    public abstract void ApplyEventToInventory(ShipInventoryService shipInventoryService);
}



