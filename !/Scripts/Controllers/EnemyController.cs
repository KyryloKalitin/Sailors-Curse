using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class EnemyController : MonoBehaviour, IDamageable
{
    public event Action<EnemyState> OnEnemyStateChanged;

    [SerializeField] private EnemyAttackZoneTriggerHandler _enemyAttackZone;
    [SerializeField] private EnemyDetectionZoneTriggerHandler _enemyDetectionZone;

    [SerializeField] private EnemySO _enemySO;
    public float HP => _currentHP;

    private float _currentHP;

    private Rigidbody _rigidbody;

    private Transform _targetPlayer;
    private PlayerStatsService _currentPlayer;

    private EnemyState _currentState
    {
        get
        {
            if (_isAttacking)
                return EnemyState.Attack;

            if (_targetPlayer != null)
                return EnemyState.Walk;

            return EnemyState.Idle;
        }
    }
    private EnemyState _lastState = EnemyState.None;

    private bool _isFalling
    {
        get
        {
            return Mathf.Abs(_rigidbody.velocity.y) >= 0.3f;
        }
    }
    private bool _isAttacking = false;

    private float _horizontalPunchForce = 6f;
    private float _verticalPunchForce = 4f;

    private Vector3 _startPosition;

    #region Lifecycle methods
    private void Awake()
    {
        _startPosition = transform.position;

        _rigidbody = GetComponent<Rigidbody>();

        _enemyAttackZone.OnPlayerEnter += _enemyAttackZone_OnPlayerEnter;
        _enemyAttackZone.OnPlayerExit += _enemyAttackZone_OnPlayerExit;

        _enemyDetectionZone.OnPlayerEnter += _enemyDetectionZone_OnPlayerEnter;
    }

    private void Start()
    {
        _currentHP = _enemySO.maxHP;
    }

    private void Update()
    {
        Rotate();
        UpdateEnemyState();
    }
    private void FixedUpdate()
    {
        Move();
    }
    #endregion Lifecycle methods

    #region Event handlers
    private void _enemyDetectionZone_OnPlayerEnter(PlayerController player)
    {
        if (_targetPlayer == null)
            _targetPlayer = player.transform;
    }
    private void _enemyAttackZone_OnPlayerEnter(PlayerStatsService player)
    {
        if (_currentPlayer != null)
            return;

        _currentPlayer = player;

        if(!_isAttacking)
            StartCoroutine(EnemyAttackCoroutine());   
    }
    private void _enemyAttackZone_OnPlayerExit(PlayerStatsService player)
    {
        if(player == _currentPlayer)
            _currentPlayer = null;
    }
    #endregion Event handlers

    public void ApplyDamage(float damage, DamageType damageType)
    {
        _currentHP -= damage;

        // For tests only
        if (_currentHP <= 0)
        {
            transform.position = _startPosition;
            _currentHP = 10f;

            return;
        }

        ApplyKnockback();
    }

    private void ApplyKnockback()
    {
        Vector3 punchForce = _targetPlayer.forward * _horizontalPunchForce + new Vector3(0f, _verticalPunchForce, 0f);
        _rigidbody.AddForce(punchForce, ForceMode.Impulse);
    }

    float animationDuration = 1f;
    private IEnumerator EnemyAttackCoroutine()
    {
        _isAttacking = true;

        yield return new WaitForSeconds(animationDuration /2);

        if (_currentPlayer != null)
            _currentPlayer.ApplyDamage(_enemySO.damage, _enemySO.damageType);

        yield return new WaitForSeconds(animationDuration / 2);

        _isAttacking = false;
        _currentPlayer = null;
    }

    private void UpdateEnemyState()
    {
        if ((_currentState != _lastState) || _lastState == EnemyState.None)
        {
            _lastState = _currentState;
            OnEnemyStateChanged?.Invoke(_currentState);
        }
    }

    private void Move()
    {
        if (_targetPlayer == null)
            return;

        if (_isAttacking)
            return;

        if (_isFalling)
            return;

        Vector3 targetVector = _targetPlayer.position - transform.position;

        Vector3 forceVector = new Vector3(targetVector.x, 0f, targetVector.z).normalized;

        _rigidbody.AddForce(_enemySO.speed * Time.fixedDeltaTime * forceVector, ForceMode.VelocityChange);

        //Vector3 clampedVelocity = new Vector3((Vector3.ClampMagnitude(_rigidbody.velocity, _maxHorizontalSpeed)).x,
        //                                        _rigidbody.velocity.y,
        //                                        (Vector3.ClampMagnitude(_rigidbody.velocity, _maxHorizontalSpeed)).z);

        //_rigidbody.velocity = clampedVelocity;
    }

    private void Rotate()
    {
        if (_targetPlayer == null)
            return;

        Vector3 targetDirection = _targetPlayer.position - transform.position;
        targetDirection.y = 0f; 

        transform.forward = targetDirection;
    }
}
