using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryService
{
    public event Action<List<InventoryItem>> OnInventoryListChanged;

    private Transform _itemHoldPoint;
    private Transform _weaponHoldPoint;
    private Transform _weaponIdleHoldPoint;

    private Weapon _weapon = null;
    private bool _weaponInventoryState = true;

    private HandheldItem _handheldItem;

    private List<InventoryItem> _inventoryItemsList;
    public int _maxItemsCount { get; private set; } = 5;
    private int _currentItemsCount;

    public PlayerInventoryService()
    {
        _inventoryItemsList = new List<InventoryItem>();
    }

    public void SetHoldPoints(IslandPlayerController.HoldPoints holdPoints)
    {
        _itemHoldPoint = holdPoints.itemHoldPoint;
        _weaponHoldPoint = holdPoints.weaponHoldPoint;
        _weaponIdleHoldPoint = holdPoints.weaponIdleHoldPoint;
    }

    public HandheldItem GetHandheldItem()
    {
        return _handheldItem;
    }
    public bool TrySetHandheldItem(HandheldItem handheldItem)
    {
        if (_handheldItem == null)
        {
            // Player has weapon in hands, let's set idle state for weapon
            SetWeaponState(false);

            // Slot for this item is free
            _handheldItem = handheldItem;
            handheldItem.SetParent(_itemHoldPoint);

            return true;
        }
        else if(handheldItem.CanBePlaced())
        {
            // Slot for this item not free, let's fix it
            _handheldItem = null;

            // Player dropped item, hands free, let's set active state for weapon
            SetWeaponState(true);

            return false;
        }
        else
        {
            // Slot for this item not free and can't fix it
            return false;
        }
    }

    public Weapon GetWeapon()
    {
        return _weapon;
    }
    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            return;

        if(_weapon == null)
        {
            _weapon = weapon;

            SetWeaponState(true);
            _weapon.TriggerState(true);
            _weapon.SetParent(_weaponHoldPoint);           

            _weapon.ResetTransform();
        }
        else
        {
            _weapon.SetParent(null);

            // Convert value from 0-360 to 180-(-180)
            float yRotation = _itemHoldPoint.rotation.eulerAngles.y;
            yRotation = (yRotation > 180f) ? yRotation - 360f : yRotation;

            _weapon.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            _weapon.transform.position = weapon.transform.position;

            _weapon.TriggerState(false);

            _weapon = null;

            SetWeapon(weapon);
        }
    }
    public bool CanWeaponAttack()
    {
        if (_weapon == null)
            return false;

        if (_weaponInventoryState && !_weapon.IsAttacking && !_weapon.IsWaitingCooldown)
            return true;
        else
            return false;
    }

    public bool TryAddToInventoryItemsList(InventoryItem item)
    {
        if (_currentItemsCount + ((InventoryItemSO)item.ItemSO).inventorySpace <= _maxItemsCount)
        {
            _currentItemsCount += ((InventoryItemSO)item.ItemSO).inventorySpace;

            _inventoryItemsList.Add(item);
            OnInventoryListChanged?.Invoke(_inventoryItemsList);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsSelectionEnabled()
    {
        if (_handheldItem == null)
            return true;
        else
            return false;
    }

    public void UnboxAll(ShipInventoryService shipInventoryService)
    {
        UnboxInventoryItems(shipInventoryService);
        TryUnboxHandheldItem(shipInventoryService);

        shipInventoryService.GetStats();

        OnInventoryListChanged?.Invoke(_inventoryItemsList);
    }

    private void UnboxInventoryItems(ShipInventoryService shipInventoryService)
    {
        if (_inventoryItemsList.Count == 0)
            return;

        for (int i = _inventoryItemsList.Count - 1; i >= 0; i--)
        {
            _inventoryItemsList[i].Unbox(shipInventoryService);
            _inventoryItemsList.RemoveAt(i);
        }
        _currentItemsCount = 0;
    }
    private bool TryUnboxHandheldItem(ShipInventoryService shipInventoryService)
    {
        if (_handheldItem is IUnboxable item)
        {
            item.Unbox(shipInventoryService);
            _handheldItem = null;

            SetWeaponState(true);

            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetWeaponState(bool state)
    {
        if (_weapon == null)
        {
            _weaponInventoryState = false;
            return;
        }

        if (state)
        {
            _weaponInventoryState = true;

            _weapon.SetParent(_weaponHoldPoint);
            _weapon.TriggerState(true);

            _weapon.ResetTransform();

            return;
        }

        if(!state)
        {
            _weaponInventoryState = false;

            _weapon.SetParent(_weaponIdleHoldPoint);
            _weapon.TriggerState(true);

            _weapon.ResetTransform();

            return;
        }
    }
}
