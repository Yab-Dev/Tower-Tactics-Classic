using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Attributes")]
    [SerializeField] private GamePhase gamePhase;
    [SerializeField] private int waveCount;
    [SerializeField] private List<GameObject> currentTowers = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField] private GameObject towerObject;
    [SerializeField] private GameObject enemyObject;

    public delegate void OnBuildPhaseChangeEventArgs();
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseStart;
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseEnd;

    public delegate void OnDefensePhaseChangeEventArgs(int waveCount);
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseStart;
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseEnd;

    public delegate void OnGetPlacedTowersEventArgs(ref List<GameObject> towers);
    public static event OnGetPlacedTowersEventArgs OnGetPlacedTowers;

    public delegate void OnCurrentTowersUpdatedEventArgs(List<GameObject> towers, List<(TraitData trait, int count)> traits);
    public static event OnCurrentTowersUpdatedEventArgs OnCurrentTowersUpdated;


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

        TowerDragDrop.OnAnyTowerMoveEnd += UpdateCurrentTowers;
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

    public void UpdateCurrentTowers()
    {
        currentTowers.Clear();
        OnGetPlacedTowers?.Invoke(ref currentTowers);

        OnCurrentTowersUpdated?.Invoke(currentTowers, GetCurrentTraits());
    }

    private List<(TraitData trait, int count)> GetCurrentTraits()
    {
        List<(TraitData trait, int count)> traitData = new List<(TraitData trait, int count)>();
        Dictionary<TraitData, int> traitDict = new Dictionary<TraitData, int>();

        foreach (GameObject tower in currentTowers)
        {
            TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
            if (towerBehavior == null) { continue; }

            List<TraitData> towerTraits = towerBehavior.GetTowerData().traits;
            foreach (TraitData trait in towerTraits)
            {
                if (traitDict.ContainsKey(trait))
                {
                    traitDict[trait]++;
                }
                else
                {
                    traitDict.Add(trait, 1);
                }
            }
        }

        foreach (var trait in traitDict.Keys)
        {
            traitData.Add((trait, traitDict[trait]));
        }

        traitData.Sort((trait1, trait2) => trait2.count.CompareTo(trait1.count));

        return traitData;
    }

    public enum GamePhase { Build, Defense, None }
}
