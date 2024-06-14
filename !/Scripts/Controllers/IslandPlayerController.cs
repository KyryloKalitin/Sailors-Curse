using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class IslandPlayerController : PlayerController
{
    [Header("Player components")]

    [SerializeField] private GroundCheckService _groundCheckService;
    [SerializeField] private PlayerAttackZoneTriggerHandler _attackZone;

    [SerializeField] private HoldPoints _holdPoints;

    [Header("Physical parameters")]

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallingAcceleration;

    private PlayerInventoryService _playerInventoryService;
    private PlayerStatsService _playerStatsService;
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

    protected override PlayerState _currentPlayerState
    {
        get
        {
            if (_playerInventoryService.GetHandheldItem() != null)
            {
                if (_isJumping)
                {
                    return PlayerState.HandheldJump;
                }

                if (_inputService.GetMovementNormalizedVector() != Vector2.zero)
                {
                    return PlayerState.HandheldWalk;
                }
                else
                {
                    return PlayerState.HandheldIdle;
                }
            }

            if (_playerInventoryService.GetWeapon() != null)
            {
                if (_playerInventoryService.GetWeapon().IsAttacking)
                {
                    return PlayerState.Attack;
                }

                if (_isJumping)
                {
                    return PlayerState.WeaponJump;
                }

                if (_inputService.GetMovementNormalizedVector() != Vector2.zero)
                {
                    return PlayerState.WeaponWalk;
                }
                else
                {
                    return PlayerState.WeaponIdle;
                }
            }

            if (_isJumping)
            {
                return PlayerState.Jump;
            }

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

    [Inject]
    public void Construct(PlayerInventoryService playerInventoryService, PlayerStatsService playerStatsService, IslandGameManager islandGameManager)
    {
        _playerStatsService = playerStatsService;
        _playerInventoryService = playerInventoryService;
        _islandGameManager = islandGameManager;

        _playerInventoryService.SetHoldPoints(_holdPoints);
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

    #region Getters
    public PlayerInventoryService GetPlayerInventoryService()
    {
        return _playerInventoryService;
    }
    public PlayerAttackZoneTriggerHandler GetAttackZone()
    {
        return _attackZone;
    }
    public PlayerStatsService GetPlayerStatsService()
    {
        return _playerStatsService;
    }
    #endregion

    #region Event handlers
    protected void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_selectedItem != null)
        {
            if(_selectedItem is TakeableItem)
            {
                TakeableItem selectedItem = _selectedItem as TakeableItem;

                selectedItem.Interact(_playerInventoryService);
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
        if (_playerInventoryService.IsSelectionEnabled())
        {
            if (item == _playerInventoryService.GetWeapon() && item != null)
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
        if (_playerInventoryService.CanWeaponAttack())
            _playerInventoryService.GetWeapon().Attack();
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

        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

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
