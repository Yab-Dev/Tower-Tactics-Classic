using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScreenUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject winScreenPrefab;
    [SerializeField] private GameObject loseScreenPrefab;



    private void Awake()
    {
        GameManager.OnGameWin += GameWin;
        GameManager.OnGameLost += GameLose;
    }

    private void OnDisable()
    {
        GameManager.OnGameWin -= GameWin;
        GameManager.OnGameLost -= GameLose;
    }

    private void GameWin()
    {
        Instantiate(winScreenPrefab, transform);
    }

    private void GameLose()
    {
        Instantiate(loseScreenPrefab, transform);
    }
}
