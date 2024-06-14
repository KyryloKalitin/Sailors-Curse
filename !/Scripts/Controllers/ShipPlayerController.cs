using UnityEngine;
using UnityEngine.InputSystem;

public class ShipPlayerController : PlayerController
{
    protected override PlayerState _currentPlayerState
    {
        get
        {
            if (_inputService.GetMovementNormalizedVector() != Vector2.zero)
            {
                return PlayerState.Walk;
            }
            else
            {
                return PlayerState.Idle;
            }
        }
    }

    private void Start()
    {
        _cameraController.OnSelectedItem += _cameraController_OnSelectedItem;

        _inputService.playerInput.Interactions.Interact.performed += Interact_performed;
        _inputService.SetMovementEnabled(true);
    }

    private void OnDestroy()
    {
        _cameraController.OnSelectedItem -= _cameraController_OnSelectedItem;
        _inputService.playerInput.Interactions.Interact.performed -= Interact_performed;

        _inputService.SetMovementEnabled(false);
    }

    protected void Interact_performed(InputAction.CallbackContext obj)
    {
        if (_selectedItem != null)
        {
            UntakeableItem selectedItem = _selectedItem as UntakeableItem;

            if(selectedItem != null)
                selectedItem.Interact();
        }
    }

    protected void _cameraController_OnSelectedItem(SelectableItem item)
    {
        _lastSelectedItem = _selectedItem;

        _selectedItem = item;

        if (_selectedItem != null)
            _selectedItem.SetSelectionState(true);

        if (_lastSelectedItem != null && _lastSelectedItem != _selectedItem)
            _lastSelectedItem.SetSelectionState(false);
    }
}
