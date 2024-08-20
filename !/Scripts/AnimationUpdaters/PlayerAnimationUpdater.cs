using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerAnimationUpdater : MonoBehaviour
{
    [SerializeField]private PlayerController _playerController;

    private Dictionary<PlayerState, string> _statesList;
    private Animator _animator;

    private void SetStateDictionary()
    {
        _statesList = new Dictionary<PlayerState, string>();

        _statesList.Add(PlayerState.Idle, "Idle");
        _statesList.Add(PlayerState.Walk, "Walk");
        _statesList.Add(PlayerState.Jump, "Jump");

        _statesList.Add(PlayerState.Attack,     "Attack");
        _statesList.Add(PlayerState.HasWeapon,  "HasWeapon");
        _statesList.Add(PlayerState.HasItem,    "HasItem");
        _statesList.Add(PlayerState.FreeHands, "FreeHands");
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        SetStateDictionary();
    }

    private void Start()
    {
        _playerController.OnChangedPlayerState += _playerController_OnChangedPlayerState;
    }

    private void _playerController_OnChangedPlayerState(CompositePlayerState state)
    {
        if(state._currentMainPlayerState != PlayerState.None)        
            _animator.SetTrigger(_statesList[state._currentMainPlayerState]);
        
        if(state._currentSecondaryPlayerState != PlayerState.None)
            _animator.SetTrigger(_statesList[state._currentSecondaryPlayerState]);
    }
}
