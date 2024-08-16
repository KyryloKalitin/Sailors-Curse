using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public partial class IslandPlayerController : PlayerController
{   
    [SerializeField] private GroundCheckService _groundCheckService;
    [SerializeField] public PlayerAttackZoneTriggerHandler AttackZone { get; private set; }

    [SerializeField] private HoldPoints _holdPoints;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallingAcceleration;

    public PlayerInventoryService PlayerInventoryService { get; private set; }
    public PlayerStatsService PlayerStatsService { get; private set; }

    private IslandGameManager _islandGameManager;

    private bool _isJumping;
    private bool _isFalling
    {
        get
        {
            return _rigidbody.velocity.y <= -0.5f;
        }
    }
    private bool _isGrounded
    {
        get
        {
            return _groundCheckService.IsGrounded;
        }
    }

    protected override void UpdatePlayerState()
    {  
        if (_isJumping)
        {
            _playerState._currentMainPlayerState = PlayerState.Jump;
        }
        else if (_inputService.GetMovementNormalizedVector() != Vector2.zero)
        {
            _playerState._currentMainPlayerState = PlayerState.Walk;
        }
        else
        {
            _playerState._currentMainPlayerState = PlayerState.Idle;
        }

        if (PlayerInventoryService.GetHandheldItem() != null)
        {
            _playerState._currentSecondaryPlayerState = PlayerState.HasItem;
        }
        else if (PlayerInventoryService.GetWeapon() != null)
        {
            if (PlayerInventoryService.GetWeapon().IsAttacking)
            {
                _playerState._currentSecondaryPlayerState = PlayerState.Attack;
            }
            else
            {
                _playerState._currentSecondaryPlayerState = PlayerState.HasWeapon;
            }
        }
        else
        {
            _playerState._currentSecondaryPlayerState = PlayerState.FreeHands;
        }
    }


    [Inject]
    public void Construct(PlayerInventoryService playerInventoryService, PlayerStatsService playerStatsService, IslandGameManager islandGameManager)
    {
        PlayerStatsService = playerStatsService;
        PlayerInventoryService = playerInventoryService;
        _islandGameManager = islandGameManager;

        PlayerInventoryService.SetHoldPoints(_holdPoints);
    }

    #region Lifecycle methods

    private void Start()
    {
        _islandGameManager.OnGameStarted += _gameManager_OnGameStarted;
        _islandGameManager.OnGameLost += _gameManager_OnGameLost;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        SetFallingAcceleration();
    }
    private void OnDestroy()
    {
        _inputService.playerInput.Movement.Jump.performed -= Jump_performed;
        _inputService.playerInput.Interactions.Interact.performed -= Interact_performed;
        _inputService.playerInput.Attack.Attack.performed -= Attack_performed;

        _cameraController.OnSelectedItem -= _cameraController_OnSelectedItem;
    }
    #endregion Lifecycle methods

    #region Event handlers
    protected void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_selectedItem != null)
        {
            if(_selectedItem is TakeableItem)
            {
                TakeableItem selectedItem = _selectedItem as TakeableItem;

                selectedItem.Interact(PlayerInventoryService);
            }

            if(_selectedItem is UntakeableItem)
            {
                UntakeableItem selectedItem = _selectedItem as UntakeableItem;

                selectedItem.Interact();
            }
        }
    }

    protected void _cameraController_OnSelectedItem(SelectableItem item)
    {
        if (PlayerInventoryService.IsSelectionEnabled())
        {
            if (item == PlayerInventoryService.GetWeapon() && item != null)
                return;

            _lastSelectedItem = _selectedItem;

            _selectedItem = item;

            if (_selectedItem != null)
                _selectedItem.SetSelectionState(true);

            if (_lastSelectedItem != null && _lastSelectedItem != _selectedItem)
                _lastSelectedItem.SetSelectionState(false);
        }
        else
        {
            if (_selectedItem != null)
                _selectedItem.SetSelectionState(false);

            if(_lastSelectedItem != null)
                _lastSelectedItem.SetSelectionState(false);
        }
    }
    private void _gameManager_OnGameStarted()
    {
        _inputService.playerInput.Movement.Jump.performed += Jump_performed;
        _inputService.playerInput.Interactions.Interact.performed += Interact_performed;
        _inputService.playerInput.Attack.Attack.performed += Attack_performed;

        _cameraController.OnSelectedItem += _cameraController_OnSelectedItem;

        _inputService.SetMovementEnabled(true);

        _inputService.SetCursorEnabled(false);
    }
    private void _gameManager_OnGameLost()
    {
        _inputService.SetMovementEnabled(false);

        _inputService.SetCursorEnabled(true);
    }
    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (PlayerInventoryService.CanWeaponAttack())
            PlayerInventoryService.GetWeapon().Attack();
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_isGrounded)
            StartCoroutine(JumpCoroutine());
    }
    #endregion Event handlers

    private IEnumerator JumpCoroutine()
    {
        _isJumping = true;

        _rigidbody.AddForce(transform.up * _jumpForce + transform.forward * _jumpForce, ForceMode.Impulse);

        // Wait for the player to jump
        yield return new WaitUntil(() => !_isGrounded);

        // Wait for the player landing
        yield return new WaitUntil(() => _isGrounded);

        _isJumping = false;
    }
    private void SetFallingAcceleration()
    {
        if (_isFalling)
        {
            _rigidbody.AddForce(-transform.up * _fallingAcceleration, ForceMode.Acceleration);
        }
    }

    [Serializable]
    public struct HoldPoints
    {
        public Transform itemHoldPoint;
        public Transform weaponHoldPoint;
        public Transform weaponIdleHoldPoint;
    }
}
