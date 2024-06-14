using UnityEngine;

public class SimpleHandheldItem : HandheldItem, IUnboxable
{
    [SerializeField] private SimpleHandheldItemSO simpleHandheldItemSO;
    public override _ItemSO ItemSO { get => simpleHandheldItemSO; }

    public void Unbox(ShipInventoryService shipInventoryService)
    {
        shipInventoryService.UpdateStats(simpleHandheldItemSO.statsList);
        
        Destroy(gameObject);
    }
}
