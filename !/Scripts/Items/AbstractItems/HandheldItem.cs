using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class HandheldItem : TakeableItem
{
    public override abstract _ItemSO ItemSO { get; }

    public bool HasParent()
    {
        return _hasParent;
    }
    public bool CanBePlaced()
    {
        return _canBePlaced;
    }

    protected bool _canBePlaced = true;
    protected bool _hasParent;

    private Transform _lastParent;

    private List<LayerMask> _ignoredLayers;

    private Collider _collider;
    private Rigidbody _rigidbody;

    public override void Interact(PlayerInventoryService inventory)
    {
        // Player interact with item
        if(inventory.TrySetHandheldItem(this))
        {
            // Player interact with item on the ground
            SetTakenItemState();
        }
        else if (_canBePlaced)
        {
            // Player interact with item in his hands, let's drop it
            SetDroppedItemState();
        }
        else
        {
            // Player interact with item in his hands but can't drop it
        }
    }
    public void SetParent(Transform newParent)
    {
        _lastParent = transform.parent;
        transform.parent = newParent;
    }

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        _ignoredLayers = new List<LayerMask>();

        _ignoredLayers.Add(LayerMask.NameToLayer("Player"));
        _ignoredLayers.Add(LayerMask.NameToLayer("Ignore Raycast"));
    }

    private void SetDroppedItemState()
    {
        _collider.isTrigger = false;
        _rigidbody.isKinematic = false;

        transform.parent = _lastParent;

        _hasParent = false;
    }
    private void SetTakenItemState()
    {
        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _hasParent = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        _canBePlaced = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        _canBePlaced = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        _canBePlaced = true;
    }

}
