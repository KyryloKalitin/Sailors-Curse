using DG.Tweening;
using UnityEngine;

public abstract class InventoryItem : SelectableItem, ITakeableItem, IUnboxable
{
    public abstract _ItemSO ItemSO { get; }

    public void Interact(PlayerInventoryService inventory, Collider collider)
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
        DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.3f))
            .Join(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + 360f, transform.rotation.z), 0.3f))
            .SetLink(gameObject)
            .OnComplete(() => Destroy(gameObject));
    }

    public abstract void Unbox(ShipZoneInventoryService shipInventory);
}
