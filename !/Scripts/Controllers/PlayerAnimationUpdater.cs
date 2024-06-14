using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationUpdater : MonoBehaviour
{
    private Dictionary<PlayerState, string> _statesList;

    private Animator _animator;
    private PlayerController _playerController;
    private void SetStateDictionary()
    {
        _statesList = new Dictionary<PlayerState, string>();

        _statesList.Add(PlayerState.Idle, "Idle");
        _statesList.Add(PlayerState.Walk, "Walk");
        _statesList.Add(PlayerState.Jump, "Jump");

        _statesList.Add(PlayerState.WeaponIdle, "WeaponIdle");
        _statesList.Add(PlayerState.WeaponWalk, "WeaponWalk");
        _statesList.Add(PlayerState.WeaponJump, "WeaponJump");
        _statesList.Add(PlayerState.Attack, "Attack");

        _statesList.Add(PlayerState.HandheldIdle, "HandheldIdle");
        _statesList.Add(PlayerState.HandheldWalk, "HandheldWalk");
        _statesList.Add(PlayerState.HandheldJump, "HandheldJump");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();

        SetStateDictionary();
    }

    private void Start()
    {
        _playerController.OnChangedPlayerState += _playerController_OnChangedPlayerState;
    }

    private void _playerController_OnChangedPlayerState(PlayerState state)
    {
        _animator.SetTrigger(_statesList[state]);
    }
}
