using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private Transform shopButtonContent;
    [SerializeField] private Button refreshShopButton;
    [SerializeField] private TMPro.TMP_Text refreshButtonCostText;
    [SerializeField] private Button upgradeCapacityButton;
    [SerializeField] private TMPro.TMP_Text upgradeCapacityCostText;

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

    private void OnDisable()
    {
        ShopManager.OnRefreshShop -= DisplayShop;
        TowerDragDrop.OnTowerPlaceStart -= DisableButtons;
        TowerDragDrop.OnTowerPlaceEnd -= EnableButtons;

        refreshShopButton.onClick.RemoveAllListeners();
        upgradeCapacityButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        refreshButtonCostText.text = ShopManager.GetInstance().GetRefreshCost().ToString();
        upgradeCapacityCostText.text = ShopManager.GetInstance().GetUpgradeCost().ToString();
    }

    private void DisplayShop(List<TowerData> _shopData)
    {
        shopButtons.Clear();
        foreach (Transform child in shopButtonContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _shopData.Count; i++)
        {
            if (_shopData[i] == null)
            {
                GameObject emptyButton = Instantiate(emptyShopButton, shopButtonContent);
                shopButtons.Add(null);
                continue;
            }

            GameObject button = Instantiate(shopButton, shopButtonContent);
            TowerShopButtonUI shopButtonUI = button.GetComponent<TowerShopButtonUI>();
            shopButtonUI.towerInfo.SetTowerInfo(_shopData[i]);
            int index = i;
            shopButtonUI.shopButton.onClick.AddListener(() => BuyTowerButtonClick(index));
            shopButtons.Add(shopButtonUI);
        }
    }

    private void BuyTowerButtonClick(int _index)
    {
        ShopManager.GetInstance().BuyTower(_index);

        DisplayShop(ShopManager.GetInstance().GetShop());
    }

    private void RefreshButtonClick()
    {
        ShopManager.GetInstance().CheckRefreshCost();
    }

    private void UpgradeCapacityButtonClick()
    {
        ShopManager.GetInstance().UpgradeCapacity();
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
