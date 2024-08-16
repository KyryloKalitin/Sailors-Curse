using System.Collections.Generic;
using UnityEngine;

public class GroundCheckService : MonoBehaviour
{
    public bool IsGrounded;

    private List<LayerMask> _ignoredLayers;
    private const string _PLAYER_LAYER = "Player";
    private const string _IGNORE_RAYCAST_LAYER = "Ignore Raycast";

    private void Awake()
    {
        _ignoredLayers = new List<LayerMask>();

        _ignoredLayers.Add(LayerMask.NameToLayer(_PLAYER_LAYER));
        _ignoredLayers.Add(LayerMask.NameToLayer(_IGNORE_RAYCAST_LAYER));
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
