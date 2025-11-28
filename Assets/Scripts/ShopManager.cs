using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;

    [Header("Attributes")]
    [SerializeField] private List<TowerData> towerPool = new List<TowerData>();
    [SerializeField] private int shopSize = 5;

    private List<TowerData> shopTowers = new List<TowerData>();

    public delegate void OnRefreshShopEventArgs(List<TowerData> shopData);
    public static event OnRefreshShopEventArgs OnRefreshShop;



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

    public void RefreshShop()
    {
        shopTowers.Clear();

        for (int i = 0; i < shopSize; i++)
        {
            shopTowers.Add(towerPool[Random.Range(0, towerPool.Count - 1)]);
        }

        OnRefreshShop?.Invoke(shopTowers);
    }

    public void BuyTower(int index)
    {
        GameManager.GetInstance().SpawnTower(true, shopTowers[index]);
        shopTowers[index] = null;
    }
}
