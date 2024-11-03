using UnityEngine;
using UnityEngine.InputSystem;

public class ShipPlayerController : PlayerController
{
    protected override void UpdatePlayerState()
    {
        if (_inputService.GetMovementNormalizedVector() != Vector2.zero)
        {
            _playerState._currentMainPlayerState = PlayerState.Walk;
        }
        else
        {
            _playerState._currentMainPlayerState = PlayerState.Idle;
        }

        _playerState._currentSecondaryPlayerState = PlayerState.FreeHands;
    }

    #region Lifecycle methods

    private void Start()
    {
        _cameraController.OnSelectedItem += _cameraController_OnSelectedItem;

        _inputService.playerInput.Interactions.Interact.performed += Interact_performed;
        _inputService.EnableMovement(true);
    }

    private void OnDestroy()
    {
        _cameraController.OnSelectedItem -= _cameraController_OnSelectedItem;
        _inputService.playerInput.Interactions.Interact.performed -= Interact_performed;
    }

    #endregion

    #region Event handlers

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (_selectedItem != null)
        {
            IUntakeableItem selectedItem = _selectedItem as IUntakeableItem;
            selectedItem?.Interact();
        }
    }

    protected void _cameraController_OnSelectedItem(SelectableItem item)
    {
        _lastSelectedItem = _selectedItem;
        _selectedItem = item;

        UpdateSelection();
    }

    #endregion

    private void UpdateSelection()
    {
        _selectedItem?.SetSelectionState(true);

        if (_lastSelectedItem != _selectedItem)
            _lastSelectedItem?.SetSelectionState(false);
    }
}
