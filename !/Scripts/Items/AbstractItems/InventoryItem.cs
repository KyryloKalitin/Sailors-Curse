using UnityEngine;

public abstract class InventoryItem : TakeableItem, IUnboxable
{
    public override abstract _ItemSO ItemSO { get; }
    public override void Interact(PlayerInventoryService inventory)
    {
        // Player interact with item
        if (inventory.TryAddToInventoryItemsList(this))
        {
            // Inventory can take this item
            SetTakenItemState();
        }
        else
        {
            // Inventory can`t take this item
            Debug.Log("Bag is full!");
        }
    }

    protected void SetTakenItemState()
    {
        Destroy(gameObject);
    }

    public abstract void Unbox(ShipInventoryService shipInventory);
}
