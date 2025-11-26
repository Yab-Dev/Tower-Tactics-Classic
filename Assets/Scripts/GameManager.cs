using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private GamePhase gamePhase;

    public delegate void OnGamePhaseChangeEventArgs();
    public static event OnGamePhaseChangeEventArgs OnBuildPhaseStart;
    public static event OnGamePhaseChangeEventArgs OnBuildPhaseEnd;
    public static event OnGamePhaseChangeEventArgs OnDefensePhaseStart;
    public static event OnGamePhaseChangeEventArgs OnDefensePhaseEnd;



    private void Start()
    {
        SetBuildPhase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetBuildPhase();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetDefensePhase();
        }
    }

    private void SetBuildPhase()
    {
        if (gamePhase == GamePhase.Defense)
        {
            OnDefensePhaseEnd?.Invoke();
        }
        gamePhase = GamePhase.Build;
        OnBuildPhaseStart?.Invoke();
    }

    private void SetDefensePhase()
    {
        if (gamePhase == GamePhase.Build)
        {
            OnBuildPhaseEnd?.Invoke();
        }
        gamePhase = GamePhase.Defense;
        OnDefensePhaseStart?.Invoke();
    }

    public enum GamePhase { Build, Defense, None }
}
