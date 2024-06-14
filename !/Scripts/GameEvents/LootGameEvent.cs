public class LootGameEvent : GameEvent
{
    public LootGameEvent(LootGameEventSO lootGameEventSO) : base(lootGameEventSO) { }

    public override void ApplyEventToInventory(ShipInventoryService shipInventoryService)
    {
        LootGameEventSO lootGameEventSO = (LootGameEventSO)eventSO;

        foreach (var item in lootGameEventSO.inventoryItemsSOList)
        {
            for (int i = 0; i < item.amount; i++)
            {
                item.inventoryItemSO.prefab.GetComponent<InventoryItem>().Unbox(shipInventoryService);
            }
        }
    }
}



