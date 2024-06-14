using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LocationSettingsSO : ScriptableObject
{
    public LocationPart[] locationPartsList = new LocationPart[3];

    [Serializable]
    public class LocationPart
    {
        [Header("Loot settings")]

        public List<Item_Chance<_ItemSO>> itemsSOList;

        public int lootPointsCount;

        public GridDensityType gridDensityType;

        public int minItemsCount;
        public int maxItemsCount;

        [Header("Environment settings")]
        public List<Item_Chance<EnvironmentItemSO>> environmentItemsSOList;

        [Range(1, 20)]
        public int environmentDensity;
        
    }
    public enum GridDensityType
    {
        Minimum,
        Medium,
        Maximum
    }
}
