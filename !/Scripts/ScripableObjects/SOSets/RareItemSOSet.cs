using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOSet/RareItems")]
public class RareItemSOSet : ScriptableObject
{
    public List<RareInventoryItemSO> rareItemSOList = new();

    public RareInventoryItemSO FindByName(string name)
    {
        foreach (var rareItem in rareItemSOList)
        {
            if (rareItem.name == name)
                return rareItem;
        }

        return null;
    }
}



