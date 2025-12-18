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



    private void Awake()
    {
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void OnDisable()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
    }

    private void SetFullscreen(bool fullscreen)
    {
        gameSettings.fullscreen = fullscreen;
    }
}
