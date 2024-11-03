using System.Collections.Generic;

[System.Serializable]
public struct LastGameEvents
{    
    public List<string> GameEventNames;

    public LastGameEvents(List<GameEventSO> gameEvents)
    {
        GameEventNames = new();

        if (gameEvents != null)
        {
            foreach (var gameEvent in gameEvents)
            {
                GameEventNames.Add(gameEvent.name);
            }
        }
    }
}
