using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public abstract class PlayerController : MonoBehaviour
{
    public event Action<CompositePlayerState> OnChangedPlayerState;

    [Header("Basic player components")]

    [SerializeField] protected FPSCameraController _cameraController;

    [Header("Basic physical parameters")]

    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxHorizontalSpeed;

    private float _yRotation = 0f;

    protected Rigidbody _rigidbody;
    protected Collider _collider;
    protected InputService _inputService;

    protected CompositePlayerState _playerState = new()
    {
        _currentMainPlayerState = PlayerState.None,
        _currentSecondaryPlayerState = PlayerState.None,
        _lastMainPlayerState = PlayerState.None,
        _lastSecondaryPlayerState = PlayerState.None
    };

    protected SelectableItem _selectedItem;
    protected SelectableItem _lastSelectedItem;

    protected abstract void UpdatePlayerState();

    #region Lifecycle methods

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        UpdatePlayerState();
        ApplyPlayerStateIfChanged();
        Rotate();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    #endregion Lifecycle methods

    private void ApplyPlayerStateIfChanged()
    {
        bool isStateChanged = _playerState._currentMainPlayerState != _playerState._lastMainPlayerState ||
                                _playerState._currentSecondaryPlayerState != _playerState._lastSecondaryPlayerState ||
                                (_playerState._lastMainPlayerState == PlayerState.None &&
                                _playerState._lastSecondaryPlayerState == PlayerState.None);

        if (isStateChanged)
        {
            bool isTryingInterruptAttack = _playerState._currentSecondaryPlayerState == PlayerState.Attack && 
                                                        _playerState._lastSecondaryPlayerState == PlayerState.Attack;

            if (isTryingInterruptAttack)
                _playerState._currentSecondaryPlayerState = PlayerState.None;
            else
                _playerState._lastSecondaryPlayerState = _playerState._currentSecondaryPlayerState;

            _playerState._lastMainPlayerState = _playerState._currentMainPlayerState;

            OnChangedPlayerState?.Invoke(_playerState);
        }
    }

    private void Move()
    {        
        Vector2 inputVector = _inputService.GetMovementNormalizedVector();

        if (inputVector == Vector2.zero)
            return;

        Vector3 forceVector = new Vector3(inputVector.x, 0f, inputVector.y);

        forceVector = transform.TransformDirection(forceVector);

        _rigidbody.AddForce(forceVector * _acceleration , ForceMode.Acceleration);

        Vector3 horizontalVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, _maxHorizontalSpeed);

        Vector3 clampedVelocity = new Vector3(horizontalVelocity.x, _rigidbody.velocity.y, horizontalVelocity.z);

        _rigidbody.velocity = clampedVelocity;
    }

    private void Rotate()
    {
        Vector2 mouseVector = _inputService.GetMouseVector();

        _yRotation += mouseVector.x * _inputService.xMouseSensitivity;

        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
