using System;
using UnityEngine;
using Zenject;

public class FPSCameraController : MonoBehaviour
{
    public event Action<SelectableItem> OnSelectedItem;

    private float _maxInteractionDistance = 2f;
    private float _xRotation;

    private LayerMask _ignoredLayersMask;

    private const string _playerLayerName = "Player";
    private const string _ignoreRaycastLayerName = "Ignore Raycast";

    private InputService _inputService;

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }

    private void Awake()
    {
        _ignoredLayersMask = ~LayerMask.GetMask(_playerLayerName, _ignoreRaycastLayerName);
    }

    private void LateUpdate()
    {
        CameraRotate();
        InteractionHandler();
    }

    private void CameraRotate()
    {
        Vector2 mouseVector = _inputService.GetMouseVector();

        _xRotation -= mouseVector.y * _inputService.yMouseSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -70f, 70f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void InteractionHandler()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxInteractionDistance, _ignoredLayersMask))
        {
            if (hit.collider.TryGetComponent <SelectableItem> (out var item))
                OnSelectedItem?.Invoke(item);
            else
                OnSelectedItem?.Invoke(null);
        }
        else
        {
            OnSelectedItem?.Invoke(null);
        }
    }
}
