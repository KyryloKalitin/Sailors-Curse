[System.Serializable]
public struct GameProgressData
{
    public ShipInventoryData ShipInventoryData;
    public PlayerStatsData PlayerStatsData;
    public LastGameEvents LastGameEvents;

    public int DaysAmount;

    public GameProgressData(ShipInventoryData shipInventoryData, PlayerStatsData playerStatsData, LastGameEvents lastGameEvents, int daysAmount)
    {
        ShipInventoryData = shipInventoryData;
        PlayerStatsData = playerStatsData;
        LastGameEvents = lastGameEvents;
        DaysAmount = daysAmount;
    }
}


