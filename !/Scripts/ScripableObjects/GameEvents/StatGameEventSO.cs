using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/StatGameEvent")]
public class StatGameEventSO : GameEventSO
{
    public List<StatsType_Saturation> statsChanged;

    public override void InitializeFromData(object data)
    {
        if(data is StatGameEventSOData statEventData)
        {
            statsChanged = statEventData.statsChanged;
            description = statEventData.description;
        }
    }
}


