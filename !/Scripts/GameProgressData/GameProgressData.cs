using System.Collections.Generic;

[System.Serializable]
public struct GameProgressData
{
    public ShipInventoryData ShipInventoryData;
    public PlayerStatsData PlayerStatsData;
    public LastGameEvents LastGameEvents;

    public int DaysAmount;

    public GameProgressData(ShipInventoryService shipInventoryService, PlayerStatsService playerStatsService, int daysAmount, List<GameEventSO> lastGameEvents = null)
    {
        ShipInventoryData = new(shipInventoryService);
        PlayerStatsData = new(playerStatsService);
        LastGameEvents = new(lastGameEvents);
        DaysAmount = daysAmount;
    }
}


