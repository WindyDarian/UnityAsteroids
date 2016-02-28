using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[System.Serializable]
public class LeaderboardEntry:IComparable<LeaderboardEntry>
{
    public string name;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public int CompareTo(LeaderboardEntry other)
    {
        return this.score.CompareTo(other.score);
    }
}

public static class LeaderboardDataManager
{

    public static List<LeaderboardEntry> highScores;
    
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/leaderboard.sav"); 
        bf.Serialize(file, highScores);
        file.Close();
    }

    public static void Load()
    {
        if (highScores != null)
        {
            return;
        }
        if (File.Exists(Application.persistentDataPath + "/leaderboard.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/leaderboard.sav", FileMode.Open);
            highScores = (List<LeaderboardEntry>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            highScores = new List<LeaderboardEntry>();
            for (int i = 0; i < 10; i++)
            {
                highScores.Add(new LeaderboardEntry("AAAAAA", 0));
            }
        }
    }

    public static void addScore(LeaderboardEntry entry)
    {
        highScores.Add(entry);
        highScores.Sort();
        highScores.Reverse();
        if (highScores.Count>10)
        {
            highScores.RemoveRange(10, highScores.Count - 10);
        }
    }
}