using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    private const string LevelTransitionTriggerName = "ScreenWipeOut";

    [Header("Attributes")]
    [SerializeField] private float transitionDuration;
    [SerializeField] private GameSettings gameSettings;

    [Header("Cache")]
    [SerializeField] private Animator transitionAnimator;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameSettings.LoadData();
    }

    public static LevelManager GetInstance()
    {
        return instance;
    }

    public void LoadLevelWithTransition(string _levelName)
    {
        transitionAnimator.SetTrigger(LevelTransitionTriggerName);

        StartCoroutine(LoadLevelDelay(_levelName, transitionDuration));
    }

    private IEnumerator LoadLevelDelay(string _levelName, float _delay)
    {
        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(_levelName);
    }
}
