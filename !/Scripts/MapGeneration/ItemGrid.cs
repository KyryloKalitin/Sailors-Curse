using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public readonly GameObject[,] itemsGrid = new GameObject[_columns, _rows];
    public float gridOffset { get; private set; }
    public float gridSize { get { return gridOffset * _columns; } }

    private const int _columns = 5;     
    private const int _rows = 5;

    public static GameObject CreateGrid(LocationSettingsSO.LocationPart locationPart, float mapPartSize)
    {
        GameObject go = new GameObject("Grid");
        ItemGrid grid = go.AddComponent<ItemGrid>();

        grid.gridOffset = grid.CalculateGridOffset(locationPart.gridDensityType, mapPartSize);

        grid.GenerateGrid(  locationPart.minItemsCount, locationPart.maxItemsCount, 
                            locationPart.itemsSOList,   grid.gridOffset);

        return go;
    }

    // Get offset between items depending on map size
    private float CalculateGridOffset(LocationSettingsSO.GridDensityType densityType, float mapPartSize)
    {
        if (densityType == LocationSettingsSO.GridDensityType.Maximum)
            return 1f;

        if (densityType == LocationSettingsSO.GridDensityType.Minimum)
            return mapPartSize/_columns;

        if(densityType == LocationSettingsSO.GridDensityType.Medium)
            return mapPartSize / 7f;

        return -1;
    }

    private void GenerateGrid(int minCount, int maxCount, List<Item_Chance<_ItemSO>> list, float itemsOffset)
    {
        minCount = Mathf.Clamp(minCount, 1, _columns * _rows);
        maxCount = Mathf.Clamp(maxCount, 1, _columns * _rows);

        int itemsCountChance = Random.Range(0, 100);
        int itemsCount;
        
        // [minCount] - 50%, (min-avarage] - 30%, [avarage-maxCount] - 20%
        if (itemsCountChance <= 50)
            itemsCount = minCount;
        else if (itemsCountChance <= 80)
            itemsCount = maxCount - minCount == 1 ? maxCount : Random.Range(minCount + 1, (maxCount+minCount)/2 + 1);
        else
            itemsCount = Random.Range((maxCount + minCount) / 2, maxCount + 1);


        for (int i = 0; i < itemsCount; i++)
        {
            int randomColumn, randomRow;

            do
            {
                // Random position  
                randomColumn = Random.Range(0, _columns);
                randomRow = Random.Range(0, _rows);

            } while (itemsGrid[randomColumn, randomRow] != null);
           
            GameObject itemGO = Instantiate(GameRandomizer.GetRandomItem(list).prefab.gameObject, transform);

            // Additional offset for random positioning to create chaos
            // Divide by 4 to get half of the initial offset, with negative and positive ranges
            float addlOffsetX = Random.Range(-itemsOffset/4, itemsOffset/4);
            float addlOffsetY = Random.Range(-itemsOffset/4, itemsOffset/4);

            Vector3 itemPosition = new Vector3(randomColumn * itemsOffset + addlOffsetX, 0f, randomRow * itemsOffset + addlOffsetY);
            itemGO.transform.localPosition = itemPosition;
            itemGO.transform.localRotation = Quaternion.Euler(0f, Random.Range(0,360), 0f);

            itemsGrid[randomColumn, randomRow] = itemGO;
        }
    }
}