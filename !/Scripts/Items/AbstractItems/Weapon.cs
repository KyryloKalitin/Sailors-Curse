using System;
using UnityEngine;

[Serializable]
public abstract class Weapon : TakeableItem
{
    [SerializeField] private Transform _ownHoldPoint;

    public override abstract _ItemSO ItemSO { get; }

    public abstract bool IsAttacking { get; protected set; }
    public abstract bool IsWaitingCooldown { get; protected set; }

    private Transform _defaultParent;
    private Collider _collider;

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider>();

        _defaultParent = transform.parent;
    }

    public override void Interact(PlayerInventoryService inventory)
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
    public void TriggerState(bool state)
    {
        _collider.isTrigger = state;
    }
    public void ResetTransform()
    {
        transform.localPosition = -_ownHoldPoint.localPosition;
        transform.localRotation = Quaternion.identity;
    }

    public abstract void Attack();
}
