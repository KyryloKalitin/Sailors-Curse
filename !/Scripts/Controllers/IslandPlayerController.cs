using System.Collections;
using UnityEngine;
using Zenject;

public partial class IslandPlayerController : PlayerController
{   
    public PlayerAttackZoneTriggerHandler AttackZone { get; private set; }
    public PlayerInventoryService PlayerInventoryService { get; private set; }
    public PlayerStatsService PlayerStatsService { get; private set; }

    [SerializeField] private GroundCheckService _groundCheckService;
    [SerializeField] private HoldPoints _holdPoints;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallingAcceleration;

    private GameTimerService _gameTimerService;

    private bool _isJumping;
    private bool _isFalling
    {
        get
        {
            return _rigidbody.velocity.y < -0.2;
        }
    }
    private bool _isGrounded
    {
        get
        {
            return _groundCheckService.IsGrounded;
        }
    }

    #region Lifecycle methods

    [Inject]
    public void Construct(PlayerInventoryService playerInventoryService, PlayerStatsService playerStatsService, GameTimerService gameTimerService)
    {
        PlayerStatsService = playerStatsService;
        PlayerInventoryService = playerInventoryService;

        _gameTimerService = gameTimerService;

        PlayerInventoryService.SetHoldPoints(_holdPoints);
    }

    private void Start()
    {
        _gameTimerService.OnCountdownStarted += _gameTimerService_OnCountdownStarted;
        _gameTimerService.OnGameplayStarted += _gameTimerService_OnGameStarted;

        _inputService.playerInput.Movement.Jump.performed += Jump_performed;
        _inputService.playerInput.Interactions.Interact.performed += Interact_performed;
        _inputService.playerInput.Attack.Attack.performed += Attack_performed;

        _cameraController.OnSelectedItem += _cameraController_OnSelectedItem;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        SetFallingAcceleration();
    }

    private void OnDestroy()
    {
        _gameTimerService.OnCountdownStarted -= _gameTimerService_OnCountdownStarted;
        _gameTimerService.OnGameplayStarted -= _gameTimerService_OnGameStarted;

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
            if(_selectedItem is ITakeableItem takeableItem)
            {
                takeableItem.Interact(PlayerInventoryService, _collider);
            }

            if(_selectedItem is IUntakeableItem untakeableItem)
            {
                untakeableItem.Interact();
            }
        }
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (PlayerInventoryService.CanWeaponAttack())
            PlayerInventoryService.Weapon.Attack();
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_isGrounded)
            StartCoroutine(JumpCoroutine());
    }

    private void _gameTimerService_OnCountdownStarted()
    {
        _inputService.EnableCameraLook(true);

        _inputService.SetCursorMode(CursorMode.Game);
    }

    private void _gameTimerService_OnGameStarted()
    {
        _inputService.EnableInteractions(true);
        _inputService.EnableMovement(true);
        _inputService.EnableAttack(true);
    }

    protected void _cameraController_OnSelectedItem(SelectableItem item)
    {
        if(!PlayerInventoryService.IsSelectionEnabled())
        {
            _selectedItem?.SetSelectionState(false);
            _lastSelectedItem?.SetSelectionState(false);
            
            return;
        }

        if (item == PlayerInventoryService.Weapon && PlayerInventoryService.Weapon != null)
            return;

        _lastSelectedItem = _selectedItem;
        _selectedItem = item;

        _selectedItem?.SetSelectionState(true);

        if (_lastSelectedItem != _selectedItem)
            _lastSelectedItem?.SetSelectionState(false);
    }

    #endregion Event handlers

    protected override void UpdatePlayerState()
    {
        UpdateMainPlayerState();
        UpdateSecondaryPlayerState();
    }

    private void UpdateMainPlayerState()
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
    }

    private void UpdateSecondaryPlayerState()
    {
        if (PlayerInventoryService.HandheldItem != null)
        {
            _playerState._currentSecondaryPlayerState = PlayerState.HasItem;
        }
        else if (PlayerInventoryService.Weapon != null)
        {
            if (PlayerInventoryService.Weapon.IsAttacking)
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
}
