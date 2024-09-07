public class ItemGameEventHandler : IConditionalGameEventHandler
{
    private ItemGameEventSO _eventSO;

    public ItemGameEventHandler(ItemGameEventSO eventSO)
    {
        _eventSO = eventSO;
    }

    public virtual void ApplyToInventory(ShipInventoryService shipInventoryService)
    {
        shipInventoryService.UpdateStats(_eventSO.statsChanged);
    }

    public virtual bool TryApplyToInventory(ShipInventoryService shipInventoryService)
    {
        if(shipInventoryService.RareItemsList.Contains(_eventSO.rareInventoryItemSO))
        {
            ApplyToInventory(shipInventoryService);
            return true;
        }
        else
        {
            return false;
        }
    }
}



