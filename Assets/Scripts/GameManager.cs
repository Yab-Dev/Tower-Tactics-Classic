using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Attributes")]
    [SerializeField] private GamePhase gamePhase;

    [Header("Prefabs")]
    [SerializeField] private GameObject towerObject;

    public delegate void OnGamePhaseChangeEventArgs();
    public static event OnGamePhaseChangeEventArgs OnBuildPhaseStart;
    public static event OnGamePhaseChangeEventArgs OnBuildPhaseEnd;
    public static event OnGamePhaseChangeEventArgs OnDefensePhaseStart;
    public static event OnGamePhaseChangeEventArgs OnDefensePhaseEnd;


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
        SetBuildPhase();
    }

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("GameManager Instance Not Found");
            return null;
        }

        return instance;
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

    public void SetBuildPhase()
    {
        if (gamePhase == GamePhase.Defense)
        {
            OnDefensePhaseEnd?.Invoke();
        }
        gamePhase = GamePhase.Build;
        OnBuildPhaseStart?.Invoke();
    }

    public void SetDefensePhase()
    {
        if (gamePhase == GamePhase.Build)
        {
            OnBuildPhaseEnd?.Invoke();
        }
        gamePhase = GamePhase.Defense;
        OnDefensePhaseStart?.Invoke();
    }

    public void SpawnTower(bool startDragging, TowerData towerData)
    {
        GameObject tower = Instantiate(towerObject, Vector2.zero, Quaternion.identity);
        TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
        TowerDragDrop towerDragDrop = tower.GetComponent<TowerDragDrop>();

        towerBehavior.SetTowerData(towerData);
        towerDragDrop.StartDraggable(startDragging);
    }

    public enum GamePhase { Build, Defense, None }
}
