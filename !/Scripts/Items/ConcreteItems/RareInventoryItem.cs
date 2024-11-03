using UnityEngine;

public class RareInventoryItem : InventoryItem
{
    public override _ItemSO ItemSO => _rareItemSO;
    [SerializeField] private RareInventoryItemSO _rareItemSO;

    public override void Unbox(ShipZoneInventoryService shipInventory)
    {
        shipInventory.UpdateRareItemsList(_rareItemSO);
    }
}
