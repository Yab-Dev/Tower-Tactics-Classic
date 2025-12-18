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
    [SerializeField] private int upgradeCost;

    private List<TowerData> shopTowers = new List<TowerData>();

    public delegate void OnRefreshShopEventArgs(List<TowerData> shopData);
    public static event OnRefreshShopEventArgs OnRefreshShop;

    public delegate void OnTowerTokensChangedEventArgs(int towerTokenAmount);
    public static event OnTowerTokensChangedEventArgs OnTowerTokensChanged;



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

    public void RefreshShop(int waveCount)
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
        RemoveTowerTokens(refreshCost);

        RefreshShop(0);
    }

    public void UpgradeCapacity()
    {
        if (upgradeCost > towerTokens || GameManager.GetInstance().GetTowerCap() >= 21)
        {
            return;
        }
        RemoveTowerTokens(upgradeCost);

        GameManager.GetInstance().IncreaseTowerCap();
    }

    public bool BuyTower(int index)
    {
        if (shopTowers[index].cost > towerTokens)
        {
            return false;
        }

        GameManager.GetInstance().SpawnTower(true, shopTowers[index]);
        RemoveTowerTokens(shopTowers[index].cost);
        shopTowers[index] = null;
        return true;
    }

    public List<TowerData> GetShop()
    {
        return shopTowers;
    }

    public void SetTowerTokens(int count)
    {
        towerTokens = count;
        OnTowerTokensChanged?.Invoke(towerTokens);
    }

    public void AddTowerTokens(int count)
    {
        towerTokens += count;
        OnTowerTokensChanged?.Invoke(towerTokens);
    }

    public void RemoveTowerTokens(int count)
    {
        towerTokens = Mathf.Max(0, towerTokens - count);
        OnTowerTokensChanged?.Invoke(towerTokens);
    }

    public int GetRefreshCost()
    {
        return refreshCost;
    }

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }

    private void StartGame()
    {
        SetTowerTokens(startingTokenCount);
    }

    private void WaveComplete(int waveCount)
    {
        AddTowerTokens(waveRewardTokenCount);
    }
}
