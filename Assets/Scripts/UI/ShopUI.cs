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
    [SerializeField] private GameObject emptyShopButton;

    private List<TowerShopButtonUI> shopButtons = new List<TowerShopButtonUI>();



    private void Awake()
    {
        ShopManager.OnRefreshShop += DisplayShop;
        TowerDragDrop.OnTowerPlaceStart += DisableButtons;
        TowerDragDrop.OnTowerPlaceEnd += EnableButtons;

        refreshShopButton.onClick.AddListener(RefreshButtonClick);
        upgradeCapacityButton.onClick.AddListener(UpgradeCapacityButtonClick);
    }

    private void DisplayShop(List<TowerData> shopData)
    {
        shopButtons.Clear();
        foreach (Transform child in shopButtonContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shopData.Count; i++)
        {
            if (shopData[i] == null)
            {
                GameObject emptyButton = Instantiate(emptyShopButton, shopButtonContent);
                shopButtons.Add(null);
                continue;
            }

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

        DisplayShop(ShopManager.GetInstance().GetShop());
    }

    private void RefreshButtonClick()
    {
        ShopManager.GetInstance().RefreshShop();
    }

    private void UpgradeCapacityButtonClick()
    {
        Debug.Log("Not implemented yet!");
    }

    public void DisableButtons()
    {
        refreshShopButton.interactable = false;
        upgradeCapacityButton.interactable = false;
        foreach (TowerShopButtonUI button in shopButtons)
        {
            if (button == null) { continue; }
            button.shopButton.interactable = false;
        }
    }

    public void EnableButtons()
    {
        refreshShopButton.interactable = true;
        upgradeCapacityButton.interactable = true;
        DisplayShop(ShopManager.GetInstance().GetShop());
    }
}
