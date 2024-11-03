using System;

[Serializable]
public struct StatsType_Saturation
{
    public StatsType statsType;
    public int statAmount;

    public StatsType_Saturation(StatsType statsType, int statAmount)
    {
        this.statsType = statsType;
        this.statAmount = statAmount;
    }
}
