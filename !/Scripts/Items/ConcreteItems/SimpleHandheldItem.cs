using UnityEngine;

public class SimpleHandheldItem : HandheldItem, IUnboxable
{
    [SerializeField] private SimpleHandheldItemSO simpleHandheldItemSO;

    public void Unbox(ShipZoneInventoryService shipInventoryService)
    {
        shipInventoryService.UpdateStats(simpleHandheldItemSO.statsList);
        
        Destroy(gameObject);
    }
}
