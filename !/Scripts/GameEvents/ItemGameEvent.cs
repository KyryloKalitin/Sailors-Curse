public class ItemGameEvent : StatGameEvent
{
    public ItemGameEvent(ItemGameEventSO itemGameEventSO) : base(itemGameEventSO) { }

    public virtual bool TryApplyEventToInventory(ShipInventoryService shipInventoryService)
    {
        ItemGameEventSO itemGameEventSO = (ItemGameEventSO)eventSO;

        if(shipInventoryService.RareItemsList.Contains(itemGameEventSO.rareInventoryItemSO))
        {
            base.ApplyEventToInventory(shipInventoryService);
            return true;
        }
        else
        {
            return false;
        }
    }
}



