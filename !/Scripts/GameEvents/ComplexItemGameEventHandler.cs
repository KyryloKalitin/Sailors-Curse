public class ComplexItemGameEventHandler : ItemGameEventHandler
{
    private ComplexItemGameEventSO _eventSO;

    public ComplexItemGameEventHandler(ComplexItemGameEventSO eventSO) : base(eventSO)
    {
        _eventSO = eventSO;
    }

    public override bool TryApplyToInventory(ShipInventoryService shipInventoryService)
    {
        if (shipInventoryService.RareItemsList.Contains(_eventSO.rareInventoryItemSO))
        {
            return true;
        }
        else
        {
            var alternativeHandler = GameEventHandlerFactory.CreateHandler(_eventSO.alternativeGameEventSO);
            alternativeHandler.ApplyToInventory(shipInventoryService);

            return false;
        }
    }
}



