using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemsStand : MonoBehaviour
{
    [SerializeField] private List<RareItemSO_GameObject> _rareItemsGOList;

    private ShipInventoryService _shipInventoryService;

    [Inject]
    public void Construct(IShipInventoryService shipInventoryService)
    {
        _shipInventoryService = (ShipInventoryService)shipInventoryService;

        _shipInventoryService.OnRareItemsListChanged += _shipInventoryService_OnRareItemsListChanged;
    }

    private void _shipInventoryService_OnRareItemsListChanged()
    {
        UpdateStandVisual();
    }

    private void Start()
    {
        UpdateStandVisual();
    }

    private void UpdateStandVisual()
    {
        DisableAllItems();

        foreach (var item in _rareItemsGOList)
        {
            if (_shipInventoryService.RareItemsList.Contains(item.rareInventoryItemSO))
                item.gameObject.SetActive(true);
        }
    }

    private void DisableAllItems()
    {
        if (_rareItemsGOList == null || _rareItemsGOList.Count == 0)
            return;

        foreach (var item in _rareItemsGOList)
        {
            item.gameObject.SetActive(false);
        }
    }

    [Serializable]
    private struct RareItemSO_GameObject
    {
        public RareInventoryItemSO rareInventoryItemSO;
        public GameObject gameObject;
    }
}
