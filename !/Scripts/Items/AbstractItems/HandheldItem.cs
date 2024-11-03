using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class HandheldItem : SelectableItem, ITakeableItem
{
    public bool CanBePlaced { get; protected set; } = true;

    private bool _isAnimating = false;

    private Transform _lastParent;

    private LayerMask _ignoredLayers;

    private Collider _collider;
    private Rigidbody _rigidbody;

    private Collider _playerCollider;

    private const float _itemThrowForce = 10f;

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        _ignoredLayers = LayerMask.GetMask("Player", "Ignore Raycast", "Weapon");
    }

    public void Interact(PlayerInventoryService inventory, Collider collider)
    {
        if (_isAnimating)
            return;

        // Player interact with item
        if(inventory.TrySetHandheldItem(this))
        {
            // Player interact with item on the ground
            SetTakenItemState();

            _playerCollider = collider;
        }
        else if (CanBePlaced)
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
        if (newParent == null)
        {
            Transform currentParent = transform.parent;

            transform.parent = _lastParent;

            _lastParent = currentParent;
        }
        else
        {
            _lastParent = transform.parent;
            transform.parent = newParent;
        }
    }

    private void SetDroppedItemState()
    {
        IgnorePlayerCollision(true);

        StartCoroutine(EnableCollisionAfterDelayCoroutine());

        _collider.isTrigger = false;
        _rigidbody.isKinematic = false;

        // Apply force to simulate a throw
        _rigidbody.AddForce(_lastParent.transform.forward * _itemThrowForce, ForceMode.Impulse);        
    }

    private IEnumerator EnableCollisionAfterDelayCoroutine()
    {
        yield return new WaitWhile(() => _playerCollider != null && _collider.bounds.Intersects(_playerCollider.bounds));

        IgnorePlayerCollision(false);

        _playerCollider = null;
    }

    private void SetTakenItemState()
    {
        TakenAnimation();

        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;
    }

    private void TakenAnimation()
    {
        _isAnimating = true;

        DOTween.Sequence()
            .Append(transform.DOLocalMove(Vector3.zero, 0.3f))
            .Join(transform.DOLocalRotateQuaternion(Quaternion.identity, 0.3f))
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                _isAnimating = false;
            });
    }

    private void OnTriggerStay(Collider other)
    {
        if ((_ignoredLayers & (1 << other.gameObject.layer)) != 0)
            return;

        CanBePlaced = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_ignoredLayers & (1 << other.gameObject.layer)) != 0)
            return;
        
        CanBePlaced = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_ignoredLayers & (1 << other.gameObject.layer)) != 0)
            return;
        
        CanBePlaced = true;
    }

    private void IgnorePlayerCollision(bool ignore)
    {
        if (_playerCollider != null)
        {
            Physics.IgnoreCollision(_playerCollider, _collider, ignore);
        }
    }
}
