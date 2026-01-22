using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopButtonUI : TooltipObject
{
    [Header("Attributes")]
    [SerializeField] private Color commonBackgroundColor;
    [SerializeField] private Color commonNameBackgroundColor;
    [SerializeField] private Color rareBackgroundColor;
    [SerializeField] private Color rareNameBackgroundColor;

    [Header("Cache")]
    [SerializeField] public TowerInfoUI towerInfo;
    [SerializeField] public Button shopButton;
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Image nameBackground;



    private void Awake()
    {
        //shopButton.onClick.AddListener(BuyTower);
    }

    private void Start()
    {
        //towerInfo.SetTowerInfo(TESTDATA);
    }

    private void BuyTower()
    {
        //GameManager.GetInstance().SpawnTower(true, TESTDATA);
        //shopButton.interactable = false;
    }

    public void SetRarityColor(TowerData.TowerRarity rarity)
    {
        switch (rarity)
        {
            case TowerData.TowerRarity.Common:
                buttonBackground.color = commonBackgroundColor;
                nameBackground.color = commonNameBackgroundColor;
                break;
            case TowerData.TowerRarity.Rare:
                buttonBackground.color = rareBackgroundColor;
                nameBackground.color = rareNameBackgroundColor;
                break;
        }
    }

    protected override void DisplayTooltip(GameObject _tooltip)
    {
        ShopTowerTooltipUI towerTooltip = _tooltip.GetComponent<ShopTowerTooltipUI>();
        if (towerTooltip != null)
        {
            towerTooltip.DisplayShopTowerData(towerInfo.TowerData);
        }
    }
}
