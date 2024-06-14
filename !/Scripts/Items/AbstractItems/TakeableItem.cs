
public abstract class TakeableItem : SelectableItem
{
    public abstract void Interact(PlayerInventoryService player);
    public abstract _ItemSO ItemSO { get; }
}
