using UnityEngine;

public static class GameEventSOFactory
{
    public static T CreateGameEventSO<T>(object data) where T: GameEventSO
    {
        T gameEventSO = ScriptableObject.CreateInstance<T>();

        //gameEventSO.InitializeFromData(data);

        

        return gameEventSO;
    }
}



