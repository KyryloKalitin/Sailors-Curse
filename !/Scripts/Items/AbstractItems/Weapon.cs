#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

[Serializable]
public abstract class Weapon : SelectableItem, ITakeableItem
{
    public abstract WeaponSO WeaponSO { get; }

    public abstract bool IsAttacking { get; protected set; }
    public abstract bool IsWaitingCooldown { get; protected set; }

    private Transform _defaultParent;
    private Collider _collider;

    [ContextMenu("Save Holding Transform")]
    public void SaveHoldingTransform()
    {
        WeaponSO.holdingPosition = transform.localPosition;
        WeaponSO.holdingRotation = transform.localRotation;

        #if UNITY_EDITOR
        EditorUtility.SetDirty(WeaponSO);
        #endif
    }

    [ContextMenu("Save Hidden Transform")]
    public void SaveHiddenTransform()
    {
        WeaponSO.hiddenPosition = transform.localPosition;
        WeaponSO.hiddenRotation = transform.localRotation;

        #if UNITY_EDITOR
        EditorUtility.SetDirty(WeaponSO);
        #endif
    }

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        var player = transform.GetComponentInParent<PlayerController>();

        if (player == null)
            _defaultParent = transform.parent;
        else
            _defaultParent = null;
    }

    public void Interact(PlayerInventoryService inventory, Collider collider)
    {
        inventory.SetWeapon(this);
    }

    public void SetParent(Transform newParent)
    {
        if (newParent == null)
        {
            transform.parent = _defaultParent;
        }
        else
        {
            transform.parent = newParent;
        }
    }
    public void SetTriggerState(bool state)
    {
        _collider.isTrigger = state;
    }

    public void ResetTransform(WeaponState weaponState)
    {
        switch (weaponState)
        {
            case WeaponState.Active:
                transform.localPosition = WeaponSO.holdingPosition;
                transform.localRotation = WeaponSO.holdingRotation;
                break;
            case WeaponState.Inactive:
                transform.localPosition = WeaponSO.hiddenPosition;
                transform.localRotation = WeaponSO.hiddenRotation;
                break;
        }    
    }

    public abstract void Attack();
}
