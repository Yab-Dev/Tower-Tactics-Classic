using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public static bool IsPaused;

    [Header("Attributes")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
    [SerializeField] private string mainMenuLevelName;

    [Header("Cache")]
    [SerializeField] private GameObject pauseMenuVisuals;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private bool isDisplayed;
    private float originalTimeScale;



    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(AbandonRun);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        IsPaused = isDisplayed;
        pauseMenuVisuals.SetActive(isDisplayed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isDisplayed = !isDisplayed;

            if (isDisplayed)
            {
                originalTimeScale = Time.timeScale;
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = originalTimeScale;
            }

            IsPaused = isDisplayed;
            pauseMenuVisuals.SetActive(isDisplayed);
        }
    }

    private void ResumeGame()
    {
        isDisplayed = false;
        IsPaused = isDisplayed;
        pauseMenuVisuals.SetActive(isDisplayed);
        Time.timeScale = originalTimeScale;
    }

    private void AbandonRun()
    {
        LevelManager.GetInstance().LoadLevelWithTransition(mainMenuLevelName);
        Time.timeScale = originalTimeScale;
    }
}
