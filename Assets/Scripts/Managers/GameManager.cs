using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Attributes")]
    [SerializeField] private GamePhase gamePhase;
    [SerializeField] private int waveCount;
    [SerializeField] private int startingLives;
    [SerializeField] private int startingTowerCap;
    [SerializeField] private List<GameObject> currentTowers = new List<GameObject>();
    [SerializeField] private TowerData startingTower;
    [SerializeField] private List<TowerSlot> startingSlots = new List<TowerSlot>();
    [SerializeField] private TooltipBaseUI tooltipObject;

    [Header("Prefabs")]
    [SerializeField] private GameObject towerObject;
    [SerializeField] private GameObject enemyObject;

    private int currentLives;
    private int towerCap;
    private bool gameLost = false;

    public delegate void OnGameStartEventArgs();
    public static event OnGameStartEventArgs OnGameStart;

    public delegate void OnGameWinEventArgs();
    public static event OnGameWinEventArgs OnGameWin;

    public delegate void OnGameLostEventArgs();
    public static event OnGameLostEventArgs OnGameLost;

    public delegate void OnBuildPhaseChangeEventArgs(int waveCount);
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseStart;
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseEnd;

    public delegate void OnDefensePhaseChangeEventArgs(int waveCount);
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseStart;
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseEnd;

    public delegate void OnGetPlacedTowersEventArgs(ref List<GameObject> towers);
    public static event OnGetPlacedTowersEventArgs OnGetPlacedTowers;

    public delegate void OnCurrentTowersUpdatedEventArgs(List<GameObject> towers, List<(TraitData trait, int count)> traits, int towerCap);
    public static event OnCurrentTowersUpdatedEventArgs OnCurrentTowersUpdated;

    public delegate void OnClearEnemiesEventArgs();
    public static event OnClearEnemiesEventArgs OnClearEnemies;


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
        TooltipBaseUI.OnAssignTooltipObject += SetTooltip;
        OnGameStart += StartGame;
        BaseBehavior.OnBaseAttacked += LoseLife;
    }

    private void OnDisable()
    {
        TowerDragDrop.OnAnyTowerMoveEnd -= UpdateCurrentTowers;
        TooltipBaseUI.OnAssignTooltipObject -= SetTooltip;
        OnGameStart -= StartGame;
        BaseBehavior.OnBaseAttacked -= LoseLife;
    }

    private void Start()
    {
        OnGameStart?.Invoke();
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

    private void StartGame()
    {
        waveCount = 0;
        currentLives = startingLives;
        towerCap = startingTowerCap;

        // Spawn starting towers
        for (int i = 0; i < startingSlots.Count; i++)
        {
            GameObject tower = SpawnTower(false, startingTower, startingSlots[i].transform.position);
            TowerDragDrop towerDragDrop = tower.GetComponent<TowerDragDrop>();

            startingSlots[i].ClearTower();
            startingSlots[i].OnSlotTowerMoved += towerDragDrop.SetStartingSlot;
            startingSlots[i].SetCurrentTower(towerDragDrop);
        }

        UpdateCurrentTowers();
        OnCurrentTowersUpdated?.Invoke(currentTowers, GetCurrentTraits(), towerCap);
        SetBuildPhase();
    }

    public void SetBuildPhase()
    {
        if (gamePhase == GamePhase.Defense)
        {
            OnDefensePhaseEnd?.Invoke(waveCount);
        }
        waveCount++;
        gamePhase = GamePhase.Build;
        OnBuildPhaseStart?.Invoke(waveCount);
    }

    public void SetDefensePhase()
    {
        if (gamePhase == GamePhase.Build)
        {
            OnBuildPhaseEnd?.Invoke(waveCount);
        }
        gamePhase = GamePhase.Defense;
        OnDefensePhaseStart?.Invoke(waveCount);
    }

    public void CompleteWave(int totalWaves)
    {
        if (waveCount == totalWaves)
        {
            OnDefensePhaseEnd?.Invoke(waveCount);
            if (!gameLost)
            {
                Debug.Log("You Win!");
                OnGameWin?.Invoke();
            }
        }
        else
        {
            SetBuildPhase();
        }
    }

    public GameObject SpawnTower(bool startDragging, TowerData towerData, Vector2 position)
    {
        GameObject tower = Instantiate(towerObject, Vector2.zero, Quaternion.identity);
        tower.transform.position = position;
        TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
        TowerDragDrop towerDragDrop = tower.GetComponent<TowerDragDrop>();

        towerBehavior.SetTowerData(towerData);
        if (startDragging)
        {
            towerDragDrop.StartDraggable(startDragging);
        }

        towerBehavior.LevelUp(1);

        return tower;
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

        OnCurrentTowersUpdated?.Invoke(currentTowers, GetCurrentTraits(), towerCap);
    }

    public List<(TraitData trait, int count)> GetCurrentTraits()
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

    private void SetTooltip(TooltipBaseUI tooltipObject)
    {
        this.tooltipObject = tooltipObject;
    }

    public TooltipBaseUI GetTooltipUI()
    {
        return tooltipObject;
    }

    private void LoseLife()
    {
        Debug.Log("Lost Life");
        currentLives--;
        if (currentLives <= 0)
        {
            GameLost();
        }
        else
        {
            OnClearEnemies?.Invoke();
        }
    }

    private void GameLost()
    {
        Debug.Log("You lose!");
        gameLost = true;
        OnClearEnemies?.Invoke();
        OnGameLost?.Invoke();
    }

    public void IncreaseTowerCap()
    {
        towerCap++;
        OnCurrentTowersUpdated?.Invoke(currentTowers, GetCurrentTraits(), towerCap);
    }

    public int GetTowerCap()
    {
        return towerCap;
    }

    public enum GamePhase { Build, Defense, None }
}
