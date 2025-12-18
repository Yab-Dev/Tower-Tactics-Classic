using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings")]
[System.Serializable]
public class GameSettings : ScriptableObject
{
    [Header("Video")]
    public bool fullscreen;

    [Header("Audio")]
    public float masterVolume;



    public void SaveData()
    {
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        fullscreen = PlayerPrefs.GetInt("fullscreen", 0) == 1 ? true : false;
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 0);
    }
}
