using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private string singleplayerLevelName;

    [Header("Cache")]
    [SerializeField] private Button singleplayerButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;



    private void Awake()
    {
        singleplayerButton.onClick.AddListener(StartSingleplayer);
        multiplayerButton.onClick.AddListener(StartMultiplayer);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        singleplayerButton.onClick.RemoveAllListeners();
        multiplayerButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    private void StartSingleplayer()
    {
        singleplayerButton.interactable = false;
        LevelManager.GetInstance().LoadLevelWithTransition(singleplayerLevelName);
    }

    private void StartMultiplayer()
    {
        multiplayerButton.interactable = false;
    }

    private void OpenSettings()
    {
        settingsButton.interactable = false;
    }

    private void QuitGame()
    {
        quitButton.interactable = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
