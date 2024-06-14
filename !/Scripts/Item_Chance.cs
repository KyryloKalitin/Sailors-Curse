using System;
using UnityEngine;

[Serializable]
public class Item_Chance<T> where T : ScriptableObject
{
    public T itemSO;

    [Range(1, 5)]
    public int spawnChance = 1;
}
