using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>


public class LevelData : MonoBehaviour
{
    private static bool needSave;

    private void Awake()
    {
        needSave = false;
    }

    private void LateUpdate()
    {
        if (needSave)
            PlayerPrefs.Save();
    }


    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }


    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        needSave = true;
    }

    public static string LoadString(string key, string defaultValue = null)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        return defaultValue;
    }
}