using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private GameSettings gameSettings;

    [Header("Cache")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Button backButton;

    public delegate void OnSettingsExitEventArgs();
    public static event OnSettingsExitEventArgs OnSettingsExit;


    private void Awake()
    {
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        backButton.onClick.AddListener(ExitSettings);
    }

    private void OnDisable()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        gameSettings.LoadData();
        fullscreenToggle.isOn = gameSettings.fullscreen;
        masterVolumeSlider.value = gameSettings.masterVolume;
    }

    private void SetFullscreen(bool fullscreen)
    {
        gameSettings.fullscreen = fullscreen;
        if (gameSettings.fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    private void SetMasterVolume(float value)
    {
        gameSettings.masterVolume = value;
    }

    private void ExitSettings()
    {
        gameSettings.SaveData();
        OnSettingsExit?.Invoke();
        Destroy(gameObject);
    }
}
