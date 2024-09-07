public class LootGameEventHandler : IGameEventHandler
{
    private LootGameEventSO _eventSO;

    public LootGameEventHandler(LootGameEventSO eventSO)
    {
        _eventSO = eventSO;
    }

    public void ApplyToInventory(ShipInventoryService shipInventoryService)
    {
        foreach (var item in _eventSO.inventoryItemsSOList)
        {
            for (int i = 0; i < item.amount; i++)
            {
                item.inventoryItemSO.prefab.GetComponent<InventoryItem>().Unbox(shipInventoryService);
            }
        }
    }
}



