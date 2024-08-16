using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationUpdater : MonoBehaviour
{
    private Animator _animator;
    private EnemyController _enemyController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();

        _enemyController.OnEnemyStateChanged += _enemyController_OnEnemyStateChanged;
    }

    private void _enemyController_OnEnemyStateChanged(EnemyState obj)
    {
        if(obj == EnemyState.Idle)
            _animator.SetTrigger("Idle");

        if (obj == EnemyState.Attack)
            _animator.SetTrigger("Attack");

        if(obj == EnemyState.Walk)
            _animator.SetTrigger("Walk");
    }
}
