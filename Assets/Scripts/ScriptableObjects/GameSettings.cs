using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings")]
[System.Serializable]
public class GameSettings : ScriptableObject
{
    private readonly string FullscreenKey = "fullscreen";
    private readonly string MasterVolumeKey = "masterVolume";
    private readonly string ResolutionWidthKey = "resolutionWidth";
    private readonly string ResolutionHeightKey = "resolutionHeight";

    [Header("Video")]
    public bool fullscreen;
    public Resolution resolution;

    [Header("Audio")]
    public float masterVolume;


    public void SaveData()
    {
        PlayerPrefs.SetInt(FullscreenKey, fullscreen ? 1 : 0);
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
        PlayerPrefs.SetInt(ResolutionWidthKey, resolution.width);
        PlayerPrefs.SetInt(ResolutionHeightKey, resolution.height);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        fullscreen = PlayerPrefs.GetInt(FullscreenKey, 1) == 1 ? true : false;
        masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1);
        resolution.width = PlayerPrefs.GetInt(ResolutionWidthKey, 0);
        resolution.height = PlayerPrefs.GetInt(ResolutionHeightKey, 0);
    }

    public void ApplySettings()
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        if (resolution.width == 0 || resolution.height == 0)
        {
            resolution.width = Screen.width;
            resolution.height = Screen.height;
        }
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
    }
}
