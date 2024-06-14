using System.Collections.Generic;
using UnityEngine;

public class GroundCheckService : MonoBehaviour
{
    public bool IsGrounded;

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

        IsGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        IsGrounded = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (_ignoredLayers.Contains(other.gameObject.layer))
            return;

        IsGrounded = true;
    }
}
