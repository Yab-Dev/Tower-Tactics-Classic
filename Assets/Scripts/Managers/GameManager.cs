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

    public delegate void OnBuildPhaseChangeEventArgs(int _waveCount);
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseStart;
    public static event OnBuildPhaseChangeEventArgs OnBuildPhaseEnd;

    public delegate void OnDefensePhaseChangeEventArgs(int _waveCount);
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseStart;
    public static event OnDefensePhaseChangeEventArgs OnDefensePhaseEnd;

    public delegate void OnGetPlacedTowersEventArgs(ref List<GameObject> _towers);
    public static event OnGetPlacedTowersEventArgs OnGetPlacedTowers;

    public delegate void OnGetTowersOfTraitEventArgs(ref List<TowerBehavior> _towers, TraitData _trait);
    public static event OnGetTowersOfTraitEventArgs OnGetTowersOfTrait;

    public delegate void OnCurrentTowersUpdatedEventArgs(List<GameObject> _towers, List<(TraitData trait, int count)> _traits, int _towerCap);
    public static event OnCurrentTowersUpdatedEventArgs OnCurrentTowersUpdated;

    public delegate void OnClearEnemiesEventArgs();
    public static event OnClearEnemiesEventArgs OnClearEnemies;

    public delegate void OnLivesChangedEventArgs(int _currentLives);
    public static event OnLivesChangedEventArgs OnLivesChanged;

    public delegate void OnClearStaticEffectsEventArgs();
    public static event OnClearStaticEffectsEventArgs OnClearStaticEffects;

    public delegate void OnApplyStaticEffectsEventArgs();
    public static event OnApplyStaticEffectsEventArgs OnApplyStaticEffects;



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
        OnLivesChanged?.Invoke(currentLives);
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
        UpdateCurrentTowers();
        OnBuildPhaseStart?.Invoke(waveCount);
    }

    public void SetDefensePhase()
    {
        if (gamePhase == GamePhase.Build)
        {
            OnBuildPhaseEnd?.Invoke(waveCount);
        }
        gamePhase = GamePhase.Defense;
        UpdateCurrentTowers();
        OnDefensePhaseStart?.Invoke(waveCount);
    }

    public void CompleteWave(int _totalWaves)
    {
        if (waveCount == _totalWaves)
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

    public GameObject SpawnTower(bool _startDragging, TowerData _towerData, Vector2 _position)
    {
        GameObject tower = Instantiate(towerObject, Vector2.zero, Quaternion.identity);
        tower.transform.position = _position;
        TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
        TowerDragDrop towerDragDrop = tower.GetComponent<TowerDragDrop>();

        towerBehavior.TowerData = _towerData;
        if (_startDragging)
        {
            towerDragDrop.StartDraggable = _startDragging;
        }

        towerBehavior.ClearStatModifications();
        towerBehavior.LevelUp(1);

        return tower;
    }

    public EnemyBehavior SpawnEnemy(EnemyData _enemyData, Vector2 _startingPos)
    {
        GameObject enemy = Instantiate(enemyObject, _startingPos, Quaternion.identity);
        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        enemyBehavior.EnemyData = _enemyData;

        return enemyBehavior;
    }

    public void UpdateCurrentTowers()
    {
        currentTowers.Clear();
        OnGetPlacedTowers?.Invoke(ref currentTowers);

        OnCurrentTowersUpdated?.Invoke(currentTowers, GetCurrentTraits(), towerCap);

        OnClearStaticEffects?.Invoke();
        OnApplyStaticEffects?.Invoke();
    }

    public List<(TraitData trait, int count)> GetCurrentTraits()
    {
        List<(TraitData trait, int count)> traitData = new List<(TraitData trait, int count)>();
        Dictionary<TraitData, int> traitDict = new Dictionary<TraitData, int>();

        foreach (GameObject tower in currentTowers)
        {
            TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
            if (towerBehavior == null) { continue; }

            List<TraitData> towerTraits = towerBehavior.TowerData.traits;
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

    private void SetTooltip(TooltipBaseUI _tooltipObject)
    {
        this.tooltipObject = _tooltipObject;
    }

    public TooltipBaseUI GetTooltipUI()
    {
        return tooltipObject;
    }

    private void LoseLife()
    {
        Debug.Log("Lost Life");
        currentLives--;
        OnLivesChanged?.Invoke(currentLives);
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

    public List<TowerBehavior> GetTowersOfTrait(TraitData _trait)
    {
        List<TowerBehavior> foundTowers = new List<TowerBehavior>();
        OnGetTowersOfTrait?.Invoke(ref foundTowers, _trait);
        return foundTowers;
    }

    public int TowerCap
    {
        get { return towerCap; }
        private set { towerCap = value; }
    }

    public enum GamePhase { Build, Defense, None }
}
