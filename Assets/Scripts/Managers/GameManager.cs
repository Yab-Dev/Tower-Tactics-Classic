using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Attributes")]
    [SerializeField] private GamePhase gamePhase;
    [SerializeField] private int waveCount;

    [Header("Prefabs")]
    [SerializeField] private GameObject towerObject;
    [SerializeField] private GameObject enemyObject;

    public delegate void OnBuildPhaseChangeEventArgs();
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseStart;
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseEnd;
    public delegate void OnDefensePhaseChangeEventArgs(int waveCount);
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseStart;
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseEnd;


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
        waveCount = 0;
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
            OnDefensePhaseEnd?.Invoke(waveCount);
        }
        waveCount++;
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
        OnDefensePhaseStart?.Invoke(waveCount);
    }

    public void CompleteWave(int totalWaves)
    {
        if (waveCount == totalWaves)
        {
            OnDefensePhaseEnd?.Invoke(waveCount);
            Debug.Log("You Win!");
        }
        else
        {
            SetBuildPhase();
        }
    }

    public void SpawnTower(bool startDragging, TowerData towerData)
    {
        GameObject tower = Instantiate(towerObject, Vector2.zero, Quaternion.identity);
        TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
        TowerDragDrop towerDragDrop = tower.GetComponent<TowerDragDrop>();

        towerBehavior.SetTowerData(towerData);
        towerDragDrop.StartDraggable(startDragging);
    }

    public EnemyBehavior SpawnEnemy(EnemyData enemyData, Vector2 startingPos)
    {
        GameObject enemy = Instantiate(enemyObject, startingPos, Quaternion.identity);
        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        enemyBehavior.SetEnemyData(enemyData);

        return enemyBehavior;
    }

    public enum GamePhase { Build, Defense, None }
}
