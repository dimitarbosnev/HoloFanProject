using System.IO;
using UnityEngine;

public static class SaveSystem<T> where T : struct
{
    static string Path()
    {
        return System.IO.Path.Combine(Application.persistentDataPath, nameof(T) + ".json");
    } 

    public static void Save(T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path(), json);
    }

    public static T Read()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, nameof(T) + ".json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"File not found: {path}");
            return new T();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}

[System.Serializable]
public struct HighScoreData
{
    public int[] scores;
}