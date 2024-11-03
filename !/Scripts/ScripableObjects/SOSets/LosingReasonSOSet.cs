using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOSet/LosingReason")]
public class LosingReasonSOSet : ScriptableObject
{
    public static LosingReason currentLosingReason;

    public List<LosingReason_Description> losingReasonsList;

    public string GetCurrentLosingReasonDescription()
    {
        if (losingReasonsList == null || losingReasonsList.Count == 0)
            return null;

        foreach (var item in losingReasonsList)
        {
            if (item.reason == currentLosingReason)
                return item.description;
        }

        return null;
    }

    [Serializable]
    public struct LosingReason_Description
    {
        public LosingReason reason;
        public string description;
    }

    public enum LosingReason
    {
        None,

        RunOutFood,
        RunOutWater,
        RunOutMaterials,

        EndTime,
        PlayerDied
    }
}



