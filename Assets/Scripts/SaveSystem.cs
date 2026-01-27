using System;
using System.IO;
using UnityEngine;

public static class SaveSystem<T> where T : struct
{
    static string Path()
    {
        return System.IO.Path.Combine(Application.dataPath, nameof(T) + ".json");
    } 

    public static void Save(T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path(), json);
    }

    public static T Read()
    {
        string path = Path();

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
    public int score1;
    public int score2;
    public int score3;
    public int score4;
    public int score5;

    public int this[int index]
    {
        get => index switch
        {
            0 => score1,
            1 => score2,
            2 => score3,
            3 => score4,
            4 => score5,
            _ => throw new IndexOutOfRangeException()
        };
        set
        {
            switch (index)
            {
                case 0: score1 = value; break;
                case 1: score2 = value; break;
                case 2: score3 = value; break;
                case 3: score4 = value; break;
                case 4: score5 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }
    }
}

