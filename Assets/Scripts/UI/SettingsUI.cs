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
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;

    public delegate void OnSettingsExitEventArgs();
    public static event OnSettingsExitEventArgs OnSettingsExit;

    private List<Resolution> currentResolutions = new List<Resolution>();


    private void Awake()
    {
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        backButton.onClick.AddListener(ExitSettings);
    }

    private void OnDisable()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        masterVolumeSlider.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        gameSettings.LoadData();
        fullscreenToggle.isOn = gameSettings.fullscreen;
        masterVolumeSlider.value = gameSettings.masterVolume;

        RefreshResolutionDropdown();

        foreach (var layoutGroup in GetComponentsInChildren<LayoutGroup>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }

    private void SetFullscreen(bool _fullscreen)
    {
        gameSettings.fullscreen = _fullscreen;
        gameSettings.ApplySettings();
    }

    private void SetMasterVolume(float _value)
    {
        gameSettings.masterVolume = _value;
        gameSettings.ApplySettings();
    }

    private void RefreshResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        currentResolutions.Clear();

        int currentResolutionIndex = 0;
        float targetAspectRatio = 16.0f / 9.0f;
        float tolerance = 0.1f;
        for (int i = 0; i < Screen.resolutions.Length; i++) 
        {
            Resolution resolution = Screen.resolutions[Screen.resolutions.Length - 1 - i];
            resolution.refreshRateRatio = new RefreshRate();

            float currentAspectRatio = (float)resolution.width / (float)resolution.height;
            if (Mathf.Abs(currentAspectRatio - targetAspectRatio) < tolerance && !currentResolutions.Contains(resolution))
            {
                if (gameSettings.resolution.width == resolution.width && gameSettings.resolution.height == resolution.height)
                {
                    currentResolutionIndex = i;
                }
                resolutionDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData($"{resolution.width} x {resolution.height}"));
                currentResolutions.Add(resolution);
            }
        }
        resolutionDropdown.value = currentResolutionIndex;
    }

    private void SetResolution(int _value)
    {
        gameSettings.resolution = currentResolutions[_value];
        gameSettings.ApplySettings();
    }

    private void ExitSettings()
    {
        gameSettings.SaveData();
        OnSettingsExit?.Invoke();
        Destroy(gameObject);
    }
}
