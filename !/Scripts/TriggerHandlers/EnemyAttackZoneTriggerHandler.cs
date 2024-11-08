using System;
using UnityEngine;

public class EnemyAttackZoneTriggerHandler : MonoBehaviour
{
    public event Action<PlayerStatsService> OnPlayerEnter;
    public event Action<PlayerStatsService> OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            OnPlayerEnter?.Invoke(playerController.PlayerStatsService);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            OnPlayerExit?.Invoke(playerController.PlayerStatsService);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            OnPlayerEnter?.Invoke(playerController.PlayerStatsService);
        }
    }
}
