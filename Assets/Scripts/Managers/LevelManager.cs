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

    public static LevelManager GetInstance()
    {
        return instance;
    }

    public void LoadLevelWithTransition(string levelName)
    {
        transitionAnimator.SetTrigger(LevelTransitionTriggerName);

        StartCoroutine(LoadLevelDelay(levelName, transitionDuration));
    }

    private IEnumerator LoadLevelDelay(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(levelName);
    }
}
