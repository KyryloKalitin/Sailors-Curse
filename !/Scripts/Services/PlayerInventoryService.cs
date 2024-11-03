using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryService
{
    public event Action<List<InventoryItem>> OnInventoryListChanged;

    public HandheldItem HandheldItem { get; private set; }
    public int MaxCapacity { get; private set; } = 5;
    public Weapon Weapon { get; private set; }

    private WeaponState _weaponState;

    public Transform ItemHoldPoint { get; private set; }
    private Transform _weaponHoldPoint;
    private Transform _weaponIdleHoldPoint;

    private List<InventoryItem> _inventoryItemsList;
    private int _currentUsedSpace;

    public PlayerInventoryService()
    {
        _inventoryItemsList = new List<InventoryItem>();
    }

    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            return;

        if(Weapon == null)
        {
            Weapon = weapon;
            Weapon.SetTriggerState(true);

            SetWeaponState(WeaponState.Active);
        }
        else
        {
            Weapon.SetParent(null);
            Weapon.SetTriggerState(false);

            // Convert value from 0-360 to 180-(-180)
            float yRotation = ItemHoldPoint.rotation.eulerAngles.y;
            yRotation = (yRotation > 180f) ? yRotation - 360f : yRotation;

            Weapon.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            Weapon.transform.position = weapon.transform.position;

            Weapon = null;

            SetWeapon(weapon);
        }
    }

    private void SetWeaponState(WeaponState weaponState)
    {
        if (Weapon == null)
            return;

        _weaponState = weaponState;

        switch(weaponState)
        {
            case WeaponState.Active:                
                Weapon.SetParent(_weaponHoldPoint);
                break;
            case WeaponState.Inactive:
                Weapon.SetParent(_weaponIdleHoldPoint);
                break;
        }

        Weapon.ResetTransform(weaponState);
    }

    public bool CanWeaponAttack()
    {
        if (Weapon == null)
            return false;

        if (Weapon.IsAttacking || Weapon.IsWaitingCooldown)
            return false;

        if (_weaponState == WeaponState.Active)
            return true;
        else
            return false;
    }

    public bool TrySetHandheldItem(HandheldItem handheldItem)
    {
        if (HandheldItem == null)
        {
            // Slot for this item is free
            HandheldItem = handheldItem;
            handheldItem.SetParent(ItemHoldPoint);

            // Player has weapon in hands, let's set inactive state for weapon
            SetWeaponState(WeaponState.Inactive);

            return true;
        }
        else if(handheldItem.CanBePlaced)
        {
            // Slot for this item not free, let's fix it
            HandheldItem = null;
            handheldItem.SetParent(null);

            // Player dropped item, hands free, let's set active state for weapon
            SetWeaponState(WeaponState.Active);

            return false;
        }
        else
        {
            // Slot for this item not free and can't fix it
            return false;
        }
    }

    public bool TryAddToInventoryItemsList(InventoryItem item)
    {
        if (_currentUsedSpace + ((InventoryItemSO)item.ItemSO).inventorySpace <= MaxCapacity)
        {
            _currentUsedSpace += ((InventoryItemSO)item.ItemSO).inventorySpace;

            _inventoryItemsList.Add(item);
            OnInventoryListChanged?.Invoke(_inventoryItemsList);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnboxAll(ShipZoneInventoryService shipInventoryService)
    {
        UnboxInventoryItems(shipInventoryService);
        UnboxHandheldItem(shipInventoryService);

        OnInventoryListChanged?.Invoke(_inventoryItemsList);
    }

    private void UnboxInventoryItems(ShipZoneInventoryService shipInventoryService)
    {
        if (_inventoryItemsList.Count == 0)
            return;

        for (int i = _inventoryItemsList.Count - 1; i >= 0; i--)
        {
            _inventoryItemsList[i].Unbox(shipInventoryService);
            _inventoryItemsList.RemoveAt(i);
        }
        _currentUsedSpace = 0;
    }

    private void UnboxHandheldItem(ShipZoneInventoryService shipInventoryService)
    {
        if (HandheldItem == null)
            return;

        if (HandheldItem is IUnboxable item)
        {
            item.Unbox(shipInventoryService);
            HandheldItem = null;

            SetWeaponState(WeaponState.Active);
        }
    }

    public void SetHoldPoints(HoldPoints holdPoints)
    {
        ItemHoldPoint = holdPoints.itemHoldPoint;
        _weaponHoldPoint = holdPoints.weaponHoldPoint;
        _weaponIdleHoldPoint = holdPoints.weaponIdleHoldPoint;
    }

    public bool IsSelectionEnabled()
    {
        if (HandheldItem == null)
            return true;
        else
            return false;
    }
}
