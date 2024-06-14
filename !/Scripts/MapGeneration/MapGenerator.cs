using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private LocationSettingsSO _locationSettingsSO;

    private const int _mapParts = 3;
    private const float _mapSize = 30f;

    private float _fixedOffset = 10f;

    private float _mapPartSize
    {
        get
        {
            return _mapSize/_mapParts;
        }
    }

    private GameObject[,] _allGrids = new GameObject[_mapParts, _mapParts];

    private List<List<GameObject>> _environmentItemsList;

    private void Awake()
    {
        _environmentItemsList = new List<List<GameObject>>();

        for (int i = 0; i < _mapParts; i++)
        {
            _environmentItemsList.Add(new List<GameObject>());
        }

        GenerateLoot();
        GenerateEnvironment();
    }

    private void GenerateLoot()
    {
        LocationSettingsSO.LocationPart locationPart;

        for (int i = 0; i < _mapParts; i++)
        {
            locationPart = _locationSettingsSO.locationPartsList[i];
            int lootPointsCount = Mathf.Clamp(locationPart.lootPointsCount, 1, _mapParts);

            for (int j = 0; j < lootPointsCount; j++)
            {
                int randSlot;

                do
                {
                    randSlot = Random.Range(0, _mapParts);
                } while (_allGrids[i, randSlot] != null);

                _allGrids[i, randSlot] = ItemGrid.CreateGrid(locationPart, _mapPartSize);

                _allGrids[i, randSlot].transform.parent = transform;

                // If grid not spread on the entire map part, it need random offset, because pivot in corner
                if (locationPart.gridDensityType == LocationSettingsSO.GridDensityType.Maximum ||
                    locationPart.gridDensityType == LocationSettingsSO.GridDensityType.Medium)
                {
                    float gridSize = _allGrids[i, randSlot].GetComponent<ItemGrid>().gridSize;

                    float excessPart = _mapPartSize - gridSize;

                    float addlOffsetX = Random.Range(0, excessPart);
                    float addlOffsetZ = Random.Range(0, excessPart);

                    _allGrids[i, randSlot].transform.localPosition = new Vector3((randSlot * _mapPartSize) + (randSlot * _fixedOffset) + addlOffsetX, 0f,
                                                                                 (i * _mapPartSize) + (i * _fixedOffset) + addlOffsetZ);
                }
                else
                {
                    _allGrids[i, randSlot].transform.localPosition = new Vector3((randSlot * _mapPartSize) + (randSlot * _fixedOffset), 0f,
                                                                                 (i * _mapPartSize) + (i * _fixedOffset));
                }
            }
        }
    }

    private void GenerateEnvironment()
    {
        LocationSettingsSO.LocationPart locationPart;

        for (int rows = 0; rows < _mapParts; rows++)
        {
            GameObject environmentGroup = new GameObject("EnvironmentGroup");
            environmentGroup.transform.parent = transform;
            environmentGroup.transform.localPosition = Vector3.zero;
           
            float minX = GetEnvironmentZoneX().x;
            float maxX = GetEnvironmentZoneX().y;

            float minZ = GetEnvironmentZoneZ(rows).x;
            float maxZ = GetEnvironmentZoneZ(rows).y;

            locationPart = _locationSettingsSO.locationPartsList[rows];

            while (true)
            {
                EnvironmentItemSO randEnvironmentItem = GameRandomizer.GetRandomItem(locationPart.environmentItemsSOList);

                if (!IsHasSpace(rows, randEnvironmentItem.size / 2f, locationPart.environmentDensity))
                    break;

                GameObject go = Instantiate(randEnvironmentItem.prefab.gameObject, environmentGroup.transform);

                int randYRotation = Random.Range(0, 5);
                go.transform.localRotation = Quaternion.Euler(new Vector3(0f, randYRotation*90, 0f));

                bool IsIntersects = true;

                while (IsIntersects)
                {
                    float randX = Random.Range(minX, maxX);
                    float randZ = Random.Range(minZ, maxZ);

                    Vector3 randPos = new Vector3(randX, 0f, randZ);

                    go.transform.localPosition = randPos;

                    IsIntersects =  IsIntersectsWithItems(rows, go.transform.position, randEnvironmentItem.size / 2f) || 
                                    IsIntersectsWithEnvironments(rows, go.transform.position, locationPart.environmentDensity, randEnvironmentItem.size / 2f);
                }

                _environmentItemsList[rows].Add(go);
            }
        }
    }

    private Vector2 GetEnvironmentZoneX()
    {
        float minX, maxX;

        minX = 0;
        maxX = _mapSize + (_mapParts - 1) * _fixedOffset;

        return new Vector2(minX, maxX);
    }

    private Vector2 GetEnvironmentZoneZ(int row)
    {
        float minZ, maxZ;

        if (row == 0)
        {
            minZ = (row * _mapPartSize) + (row * _fixedOffset);
            maxZ = (row * _mapPartSize) + (row * _fixedOffset) + _mapPartSize + _fixedOffset / 2;

        }
        else if (row == _mapParts - 1)
        {
            minZ = (row * _mapPartSize) + (row * _fixedOffset) - _fixedOffset / 2;
            maxZ = (row * _mapPartSize) + (row * _fixedOffset) + _mapPartSize;
        }
        else
        {
            minZ = (row * _mapPartSize) + (row * _fixedOffset) - _fixedOffset / 2;
            maxZ = (row * _mapPartSize) + (row * _fixedOffset) + _mapPartSize + _fixedOffset / 2;
        }

        return new Vector2(minZ, maxZ);
    }

    private bool IsHasSpace(int row, float itemRadius, float minDistance)
    {
        for (float x = GetEnvironmentZoneX().x; x < GetEnvironmentZoneX().y; x++)
        {
            for (float z = GetEnvironmentZoneZ(row).x; z < GetEnvironmentZoneZ(row).y; z++)
            {
                Vector3 currentPos = transform.TransformPoint(new Vector3(x, 0f, z));

                if (!IsIntersectsWithItems(row, currentPos, itemRadius) && 
                    !IsIntersectsWithEnvironments(row, currentPos, minDistance, itemRadius))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsIntersectsWithItems(int row, Vector3 randPos, float itemRadius)
    {
        for (int column = 0; column < _mapParts; column++)
        {
            if (_allGrids[row, column] == null)
                continue;

            ItemGrid currentGrid = _allGrids[row, column].GetComponent<ItemGrid>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (currentGrid.itemsGrid[i, j] == null)
                        continue;

                    Vector3 itemPos = currentGrid.itemsGrid[i, j].transform.position;

                    float distance = (itemPos - randPos).magnitude;

                    if (distance < 0.5f + itemRadius)
                        return true;
                }
            }
        }

        return false;
    }

    private bool IsIntersectsWithEnvironments(int row, Vector3 randPos, float minDistance, float itemRadius)
    {
        foreach (var item in _environmentItemsList[row])
        {
            float currentRadius = item.GetComponent<EnvironmentItem>().GetEnvironmentItemSO().size / 2f;

            float distance = (item.transform.position - randPos).magnitude;

            if (distance < itemRadius + currentRadius + minDistance)
                return true;
        }

        if (row == 0)
            return false;

        foreach (var item in _environmentItemsList[row-1])
        {
            float currentRadius = item.GetComponent<EnvironmentItem>().GetEnvironmentItemSO().size / 2f;

            float distance = (item.transform.position - randPos).magnitude;

            if (distance < itemRadius + currentRadius + minDistance)
                return true;
        }

        return false;
    }
}
