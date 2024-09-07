using UnityEngine;

public abstract class GameEventSO : ScriptableObject
{
    public string description;

    public abstract void InitializeFromData(object data);
}
