using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackZoneTriggerHandler : MonoBehaviour
{
    public event Action<IDamageable> OnDamageableDetected;

    private List<LayerMask> _ignoredLayers;

    private void Awake()
    {
        _ignoredLayers = new List<LayerMask>();

        _ignoredLayers.Add(LayerMask.NameToLayer("Player"));
        _ignoredLayers.Add(LayerMask.NameToLayer("Ignore Raycast"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        if (other.gameObject.TryGetComponent(out IDamageable enemy))
        {
            OnDamageableDetected?.Invoke(enemy);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        if (other.gameObject.TryGetComponent(out IDamageable enemy))
        {
            OnDamageableDetected?.Invoke(enemy);
        }
    }
}
