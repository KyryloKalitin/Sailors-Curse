using UnityEngine;

public class ContainerHandheldItem : HandheldItem, IUnboxable
{
    [SerializeField] private ContainerHandheldItemSO containerHandheldItemSO;

    public void Unbox(ShipZoneInventoryService shipInventoryService)
    {
        Destroy(gameObject);
    }
}
