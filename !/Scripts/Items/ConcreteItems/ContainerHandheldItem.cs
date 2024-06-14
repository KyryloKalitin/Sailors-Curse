using UnityEngine;

public class ContainerHandheldItem : HandheldItem, IUnboxable
{
    [SerializeField] private ContainerHandheldItemSO containerHandheldItemSO;
    public override _ItemSO ItemSO { get => containerHandheldItemSO; }

    public void Unbox(ShipInventoryService shipInventoryService)
    {
        Destroy(gameObject);
    }
}
