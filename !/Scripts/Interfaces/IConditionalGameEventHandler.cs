public interface IConditionalGameEventHandler : IGameEventHandler
{
    bool TryApplyToInventory(ShipInventoryService shipInventoryService);
}