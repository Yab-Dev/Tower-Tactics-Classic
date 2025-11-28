using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private Transform shopButtonContent;
    [SerializeField] private Button refreshShopButton;
    [SerializeField] private Button upgradeCapacityButton;

    [Header("Prefabs")]
    [SerializeField] private GameObject shopButton;

    private List<TowerShopButtonUI> shopButtons = new List<TowerShopButtonUI>();



    private void Awake()
    {
        ShopManager.OnRefreshShop += RefreshShop;

        refreshShopButton.onClick.AddListener(RefreshButtonClick);
        upgradeCapacityButton.onClick.AddListener(UpgradeCapacityButtonClick);
    }

    private void RefreshShop(List<TowerData> shopData)
    {
        shopButtons.Clear();
        foreach (Transform child in shopButtonContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shopData.Count; i++)
        {
            GameObject button = Instantiate(shopButton, shopButtonContent);
            TowerShopButtonUI shopButtonUI = button.GetComponent<TowerShopButtonUI>();
            shopButtonUI.towerInfo.SetTowerInfo(shopData[i]);
            int index = i;
            shopButtonUI.shopButton.onClick.AddListener(() => BuyTowerButtonClick(index));
            shopButtons.Add(shopButtonUI);
        }
    }

    private void BuyTowerButtonClick(int index)
    {
        ShopManager.GetInstance().BuyTower(index);
        shopButtons[index].shopButton.interactable = false;
    }

    private void RefreshButtonClick()
    {
        ShopManager.GetInstance().RefreshShop();
    }

    private void UpgradeCapacityButtonClick()
    {
        Debug.Log("Not implemented yet!");
    }
}
