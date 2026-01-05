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

    private List<TowerData> shopTowers = new List<TowerData>();

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

        GameManager.OnBuildPhaseStart += RefreshShop;
        GameManager.OnGameStart += StartGame;
        GameManager.OnDefensePhaseEnd += WaveComplete;
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= RefreshShop;
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

        for (int i = 0; i < shopSize; i++)
        {
            shopTowers.Add(towerPool[Random.Range(0, towerPool.Count)]);
        }

        OnRefreshShop?.Invoke(shopTowers);
    }

    public void CheckRefreshCost()
    {
        if (refreshCost > towerTokens)
        {
            return;
        }
        TowerTokens -= refreshCost;

        RefreshShop(0);
    }

    public void UpgradeCapacity()
    {
        if (upgradeCost > towerTokens || GameManager.GetInstance().TowerCap >= 21)
        {
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
