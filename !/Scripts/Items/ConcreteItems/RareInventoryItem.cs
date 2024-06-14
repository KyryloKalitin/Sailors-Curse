using UnityEngine;

public class RareInventoryItem : InventoryItem
{
    [SerializeField] private RareInventoryItemSO _rareItemSO;
    public override _ItemSO ItemSO { get => _rareItemSO; }
    public override void Unbox(ShipInventoryService shipInventory)
    {
        shipInventory.UpdateRareItemsList(this);
    }
}
