using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMenuUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private string gameplayLevelName;
    [SerializeField] private string mainMenuLevelName;

    [Header("Cache")]
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;



    private void Awake()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    private void OnDisable()
    {
        playAgainButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
    }

    private void PlayAgain()
    {
        LevelManager.GetInstance().LoadLevelWithTransition(gameplayLevelName);
    }

    private void MainMenu()
    {
        LevelManager.GetInstance().LoadLevelWithTransition(mainMenuLevelName);
    }
}
