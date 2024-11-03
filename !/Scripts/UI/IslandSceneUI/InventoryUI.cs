using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private Transform _itemPrefab;

    private List<Transform> _slotsList;

    private PlayerInventoryService _playerInventoryService;

    [Inject]
    public void Construct(PlayerInventoryService playerInventoryService)
    {
        _playerInventoryService = playerInventoryService;
    }

    private void Start()
    {
        _slotsList = new();

        _playerInventoryService.OnInventoryListChanged += _playerInventoryService_OnInventoryListChanged;

        RefreshInventoryUI();
    }


    private void RefreshInventoryUI()
    {
        foreach (Transform child in _itemContainer)
        {
            Destroy(child.gameObject);

            _slotsList.Clear();
        }

        for (int i = 0; i < _playerInventoryService.MaxCapacity; i++)
        {
            _slotsList.Add(Instantiate(_itemPrefab, _itemContainer));
        }
    }

    private void _playerInventoryService_OnInventoryListChanged(List<InventoryItem> inventoryItems)
    {
        RefreshInventoryUI();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItemSO inventoryItemSO = (InventoryItemSO)inventoryItems[i].ItemSO;

            for (int j = 0; j < inventoryItemSO.inventorySpace; j++)
            {  
                _slotsList[GetItemsSpaceBefore(inventoryItems, i) + j].GetComponent<SingleInventoryItemUI>().SetIcon(inventoryItemSO.sprite);
            }
        }
    }

    private int GetItemsSpaceBefore(List<InventoryItem> inventoryItems, int current)
    {
        int result = 0;

        for (int i = 0; i < current; i++)
        {
            InventoryItemSO inventoryItemSO = (InventoryItemSO)inventoryItems[i].ItemSO;
            result += inventoryItemSO.inventorySpace;
        }

        return result;
    }
}
