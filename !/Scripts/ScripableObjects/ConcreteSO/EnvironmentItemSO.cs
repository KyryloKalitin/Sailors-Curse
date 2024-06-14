using UnityEngine;

[CreateAssetMenu()]
public class EnvironmentItemSO : ScriptableObject
{
    public Transform prefab;

    [Range(1,3)]
    public int size = 1;
}
