using System.Collections.Generic;

[System.Serializable]
public struct PlayerStatsData
{
    public float HP;
    public List<TypedHit> typedHitsList;
    public PlayerStatsData(PlayerStatsService playerStatsService)
    {
        HP = playerStatsService.HP;
        typedHitsList = playerStatsService.TypedHitsList;
    }
}

