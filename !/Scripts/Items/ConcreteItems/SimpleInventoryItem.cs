using UnityEngine;

public class SimpleInventoryItem : InventoryItem
{
    [SerializeField] private SimpleInventoryItemSO simpleItemSO;
    public override _ItemSO ItemSO { get => simpleItemSO; }

    public override void Unbox(ShipInventoryService shipInventory)
    {
        shipInventory.UpdateStats(simpleItemSO.statsList);
    }
}
