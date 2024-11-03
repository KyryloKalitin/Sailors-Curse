using System.Collections.Generic;

public static class GameEventSOExtensions
{
    public static void DeserializeFromData(this List<GameEventSO> gameEventSOList, LastGameEvents lastGameEvents)
    {        
        if (lastGameEvents.GameEventNames == null)
            return;

        gameEventSOList.Clear();

        var eventSet = SOSetLoader.LoadGameEventSOSet();

        foreach (var gameEvent in lastGameEvents.GameEventNames)
        {
            GameEventSO currentEvent = eventSet.FindEventByName(gameEvent);

            if (currentEvent != null)
                gameEventSOList.Add(currentEvent);
        }
    }
}
