using System;
using UnityEngine;

public class EnemyDetectionZoneTriggerHandler : MonoBehaviour
{
    public event Action<PlayerController> OnPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            OnPlayerEnter?.Invoke(playerController);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            OnPlayerEnter?.Invoke(playerController);
        }
    }
}
