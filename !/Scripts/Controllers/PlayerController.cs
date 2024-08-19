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

    [SerializeField] protected float _movementSpeed;
    [SerializeField] protected float _maxHorizontalSpeed;

    protected float _yRotation = 0f;

    protected Rigidbody _rigidbody;
    protected InputService _inputService;

    protected CompositePlayerState _playerState = new()
    {
        _currentMainPlayerState = PlayerState.None,
        _currentSecondaryPlayerState = PlayerState.None,
        _lastMainPlayerState = PlayerState.None,
        _lastSecondaryPlayerState = PlayerState.None
    };
    protected abstract void UpdatePlayerState();

    protected SelectableItem _selectedItem = null;
    protected SelectableItem _lastSelectedItem = null;

    #region Lifecycle methods

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        ApplyPlayerStateIfChanged();
        Rotate();
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    #endregion Lifecycle methods

    protected void ApplyPlayerStateIfChanged()
    {
        UpdatePlayerState();

        if (_playerState._currentMainPlayerState != _playerState._lastMainPlayerState ||
           _playerState._currentSecondaryPlayerState != _playerState._lastSecondaryPlayerState ||
           _playerState._lastMainPlayerState == PlayerState.None || _playerState._lastSecondaryPlayerState == PlayerState.None)
        {
            _playerState._lastMainPlayerState = _playerState._currentMainPlayerState;

            _playerState._lastSecondaryPlayerState = _playerState._currentSecondaryPlayerState;

            Debug.Log(_playerState._currentMainPlayerState + " " + _playerState._currentSecondaryPlayerState);
            OnChangedPlayerState?.Invoke(_playerState);
        }
    }

    protected void Move()
    {
        Vector2 inputVector = _inputService.GetMovementNormalizedVector();
        Vector3 forceVector = new Vector3(inputVector.x, 0f, inputVector.y);

        forceVector = transform.TransformDirection(forceVector);

        _rigidbody.AddForce(forceVector * _movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        Vector3 clampedVelocity = new Vector3(  (Vector3.ClampMagnitude(_rigidbody.velocity, _maxHorizontalSpeed)).x, 
                                                _rigidbody.velocity.y, 
                                                (Vector3.ClampMagnitude(_rigidbody.velocity, _maxHorizontalSpeed)).z);

        _rigidbody.velocity = clampedVelocity;
    }

    protected void Rotate()
    {
        Vector2 mouseVector = _inputService.GetMouseVector();

        _yRotation += mouseVector.x * _inputService.xMouseSensitivity;

        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
