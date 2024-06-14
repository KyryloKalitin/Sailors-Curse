using System;
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

        if(File.Exists(_gameProgressDataPath))
        {
            string jsonData = File.ReadAllText(_gameProgressDataPath);

            gameProgressData = JsonUtility.FromJson<GameProgressData>(jsonData);
        }
        else
        {
            gameProgressData = GameProgressData.CreateDefault();

            SaveData(gameProgressData);
        }

        return gameProgressData;
    }

    public static void DeleteGameProgressData()
    {
        File.Delete(_gameProgressDataPath);
    }
}
