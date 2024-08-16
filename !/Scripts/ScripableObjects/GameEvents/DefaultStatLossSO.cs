using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Stat Loss")]
public class DefaultStatLossSO : ScriptableObject
{
    public List<StatsType_Saturation> statsChanged;
}
