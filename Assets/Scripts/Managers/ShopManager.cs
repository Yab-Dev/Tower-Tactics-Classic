using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;

    [Header("Attributes")]
    [SerializeField] private List<TowerData> towerPool = new List<TowerData>();
    [SerializeField] private int shopSize = 5;
    [SerializeField] private int towerTokens;
    [SerializeField] private int startingTokenCount;
    [SerializeField] private int waveRewardTokenCount;
    [SerializeField] private int refreshCost;
    [SerializeField] private int startingUpgradeCost;
    [SerializeField] private int upgradeCost;
    [SerializeField] private float upgradeCostScalar;
    [SerializeField] private float rareTowerOdds;
    [SerializeField] private string notEnoughTokensMessage;
    [SerializeField] private Color notEnoughTokensColor;

    [Header("Cache")]
    [SerializeField] private TraitData earthyTrait;
    [SerializeField] private TraitData crystallizedTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject textPopupPrefab;

    private Dictionary<TowerData.TowerRarity, List<TowerData>> towersWithRarities = new Dictionary<TowerData.TowerRarity, List<TowerData>>();
    private List<TowerData> shopTowers = new List<TowerData>();

    private bool firstShopOfPhase;

    public delegate void OnRefreshShopEventArgs(List<TowerData> _shopData);
    public static event OnRefreshShopEventArgs OnRefreshShop;

    public delegate void OnTowerTokensChangedEventArgs(int _towerTokenAmount);
    public static event OnTowerTokensChangedEventArgs OnTowerTokensChanged;

    public delegate void OnUpgradeCostChangedEventArgs(int _upgradeCostAmount);
    public static event OnUpgradeCostChangedEventArgs OnUpgradeCostChanged;



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

        towersWithRarities.Add(TowerData.TowerRarity.Common, new List<TowerData>());
        towersWithRarities.Add(TowerData.TowerRarity.Rare, new List<TowerData>());
        foreach (TowerData tower in towerPool)
        {
            towersWithRarities[tower.rarity].Add(tower);
        }

        //GameManager.OnBuildPhaseStart += StartBuildPhase;
        GameManager.OnGameStart += StartGame;
        GameManager.OnDefensePhaseEnd += WaveComplete;
    }

    private void OnDisable()
    {
        //GameManager.OnBuildPhaseStart -= StartBuildPhase;
        GameManager.OnGameStart -= StartGame;
        GameManager.OnDefensePhaseEnd -= WaveComplete;
    }

    public static ShopManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("ShopManager Instance Not Found");
            return null;
        }

        return instance;
    }

    public void RefreshShop(int _waveCount)
    {
        shopTowers.Clear();

        int towersToPopulate = shopSize;

        GameManager.GetInstance().UpdateCurrentTowers();

        Debug.Log($"First Shop: {firstShopOfPhase}");

        if (firstShopOfPhase && TraitUtils.CheckTraitBreakpoint(crystallizedTrait, 0))
        {
            Debug.Log("EARTHY CONFIRM");

            List<TowerData> earthyTowers = new List<TowerData>();
            foreach (TowerData tower in towerPool)
            {
                if (tower.traits.Contains(earthyTrait))
                {
                    earthyTowers.Add(tower);
                }
            }
            shopTowers.Add(earthyTowers[Random.Range(0, earthyTowers.Count)]);
            towersToPopulate--;
        }

        for (int i = 0; i < towersToPopulate; i++)
        {
            float rand = Random.Range(0.0f, 1.0f);
            if (rand < rareTowerOdds)
            {
                shopTowers.Add(towersWithRarities[TowerData.TowerRarity.Rare][Random.Range(0, towersWithRarities[TowerData.TowerRarity.Rare].Count)]);
            }
            else
            {
                shopTowers.Add(towersWithRarities[TowerData.TowerRarity.Common][Random.Range(0, towersWithRarities[TowerData.TowerRarity.Common].Count)]);
            }
        }

        OnRefreshShop?.Invoke(shopTowers);

        firstShopOfPhase = false;
    }

    public void StartBuildPhase(int _waveCount)
    {
        firstShopOfPhase = true;
        RefreshShop(0);
    }

    public void CheckRefreshCost()
    {
        if (refreshCost > towerTokens)
        {
            TextPopupUI.CreateTextPopup(textPopupPrefab, notEnoughTokensMessage, notEnoughTokensColor);
            return;
        }
        TowerTokens -= refreshCost;

        RefreshShop(0);
    }

    public void UpgradeCapacity()
    {
        if (upgradeCost > towerTokens || GameManager.GetInstance().TowerCap >= 21)
        {
            TextPopupUI.CreateTextPopup(textPopupPrefab, notEnoughTokensMessage, notEnoughTokensColor);
            return;
        }
        TowerTokens -= upgradeCost;

        GameManager.GetInstance().IncreaseTowerCap();

        float cost = UpgradeCost;
        cost *= upgradeCostScalar;
        UpgradeCost = Mathf.RoundToInt(cost);
    }

    public bool BuyTower(int _index)
    {
        if (shopTowers[_index].cost > towerTokens)
        {
            TextPopupUI.CreateTextPopup(textPopupPrefab, notEnoughTokensMessage, notEnoughTokensColor);
            return false;
        }

        GameManager.GetInstance().SpawnTower(true, shopTowers[_index], Vector2.zero);
        TowerTokens -= shopTowers[_index].cost;
        shopTowers[_index] = null;
        return true;
    }

    private void StartGame()
    {
        TowerTokens = startingTokenCount;
        UpgradeCost = startingUpgradeCost;
    }

    private void WaveComplete(int _waveCount)
    {
        TowerTokens += waveRewardTokenCount;
    }

    public List<TowerData> Shop
    {
        get { return shopTowers; }
        private set { shopTowers = value; }
    }

    public int TowerTokens
    {
        get { return towerTokens; }
        set 
        { 
            towerTokens = value; 
            towerTokens = Mathf.Max(0, towerTokens);
            OnTowerTokensChanged?.Invoke(towerTokens);
        }
    }

    public int RefreshCost
    {
        get { return refreshCost; }
        private set {  refreshCost = value; }
    }

    public int UpgradeCost
    {
        get { return upgradeCost; }
        private set 
        {
            upgradeCost = value; 
            OnUpgradeCostChanged?.Invoke(upgradeCost);
        }
    }
}
