using UnityEngine;

public class SimpleInventoryItem : InventoryItem
{
    public override _ItemSO ItemSO => simpleItemSO;
    [SerializeField] private SimpleInventoryItemSO simpleItemSO;

    public override void Unbox(ShipZoneInventoryService shipInventory)
    {
        shipInventory.UpdateStats(simpleItemSO.statsList);
    }
}
