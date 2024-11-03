using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameProgressDataIO
{
    public static event Action<GameProgressData> OnDataSaved;

    private static string _gameProgressDataPath = Application.persistentDataPath + "/gameProgressData.json";

    public static void SaveData(GameProgressData gameProgressData)
    {
        string jsonData = JsonUtility.ToJson(gameProgressData);

        File.WriteAllText(_gameProgressDataPath, jsonData);

        OnDataSaved?.Invoke(gameProgressData);
    }

    public static GameProgressData LoadData()
    {
        GameProgressData gameProgressData;

        if(HasSavedGameProgress())
        {
            string jsonData = File.ReadAllText(_gameProgressDataPath);

            gameProgressData = JsonUtility.FromJson<GameProgressData>(jsonData);
        }
        else
        {
            gameProgressData = new(new ShipInventoryService(), new PlayerStatsService(), 1, new List<GameEventSO>());

            SaveData(gameProgressData);
        }

        return gameProgressData;
    }

    public static bool HasSavedGameProgress()
    {
        return File.Exists(_gameProgressDataPath);
    }

    public static void DeleteGameProgressData()
    {
        File.Delete(_gameProgressDataPath);
    }
}
