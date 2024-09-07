using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent/DefaultStatLoss")]
public class DefaultStatLossSO : ScriptableObject
{
    public List<StatsType_Saturation> statsChanged;
}
