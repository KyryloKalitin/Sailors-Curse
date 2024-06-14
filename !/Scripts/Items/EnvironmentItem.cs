using UnityEngine;

public class EnvironmentItem : MonoBehaviour
{
    [SerializeField] private EnvironmentItemSO environmentItemSO;

    public EnvironmentItemSO GetEnvironmentItemSO()
    {
        return environmentItemSO;
    }
}
