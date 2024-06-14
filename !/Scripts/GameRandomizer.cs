using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GameRandomizer
{
    public static T GetRandomItem<T>(List<Item_Chance<T>> itemsList) where T : ScriptableObject
    {
        if (itemsList.Count == 0)
            return null;

        int rarityIndex = GetRarityIndex();

        List<Item_Chance<T>> listForRarity = new List<Item_Chance<T>>();

        while (listForRarity.Count == 0)
        {
            listForRarity = itemsList.FindAll(item => item.spawnChance == rarityIndex);

            rarityIndex++;
        }

        T resultItem = listForRarity[Random.Range(0, listForRarity.Count)].itemSO;

        return resultItem;
    }

    private static int GetRarityIndex()
    {
        int randomIndex = Random.Range(0, 100);

        if (randomIndex <= 40)
            return 5;
        else if (randomIndex <= 70)
            return 4;
        else if (randomIndex <= 90)
            return 3;
        else if (randomIndex <= 97)
            return 2;
        else
            return 1;
    }
}
