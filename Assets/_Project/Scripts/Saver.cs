using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public static class Saver
{
    public static void SaveHighScore(int highScore)
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public static void SaveSettings(SettingsVars settingsVars)
    {
        PlayerPrefs.SetInt("SoundEnabled", settingsVars.soundEnabled?1:0);
        PlayerPrefs.SetInt("MusicEnabled", settingsVars.musicEnabled?1:0);
    }

    public static void LoadSettings(SettingsVars settingsVars)
    {
        settingsVars.soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1 ? true : false;
        settingsVars.musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1 ? true : false;
    }

    public static void SaveSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
    }

    public static int LoadSkin()
    {
        return PlayerPrefs.GetInt("SkinIndex", 0);
    }

    public static void SaveAdsWatchedData(int[] array)
    {
        MemoryStream stream = new MemoryStream();
        new BinaryFormatter().Serialize(stream, array);
        string str = Convert.ToBase64String(stream.ToArray());
        stream.Close();
        PlayerPrefs.SetString("AdsWatched", str);
    }

    public static int[] LoadAdsWatchedData()
    {
        string str = PlayerPrefs.GetString("AdsWatched");
        byte[] bytes = Convert.FromBase64String(str);
        MemoryStream stream = new MemoryStream(bytes);
        if (stream.Length == 0)
            return new int[] { 0, 0, 0, 0 };  // DEFAULT VALUE
        int[] array = (int[]) new BinaryFormatter().Deserialize(stream);
        return array;
    }

    public static void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
