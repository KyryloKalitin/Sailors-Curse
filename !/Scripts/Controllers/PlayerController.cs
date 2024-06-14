using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public abstract class PlayerController : MonoBehaviour
{
    public event Action<PlayerState> OnChangedPlayerState;

    [Header("Player components")]

    [SerializeField] protected FPSCameraController _cameraController;

    [Header("Physical parameters")]

    [SerializeField] protected float _movementSpeed;
    [SerializeField] protected float _maxHorizontalSpeed;

    private float _yRotation = 0f;

    protected Rigidbody _rigidbody;
    protected InputService _inputService;

    protected abstract PlayerState _currentPlayerState { get; }
    protected PlayerState _lastPlayerState = PlayerState.None;

    protected SelectableItem _selectedItem = null;
    protected SelectableItem _lastSelectedItem = null;

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }

    #region Lifecycle methods
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        UpdatePlayerState();
        Rotate();
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    #endregion Lifecycle methods

    protected void UpdatePlayerState()
    {
        if ((_currentPlayerState != _lastPlayerState) || _lastPlayerState == PlayerState.None)
        {
            _lastPlayerState = _currentPlayerState;
            OnChangedPlayerState?.Invoke(_currentPlayerState);
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
