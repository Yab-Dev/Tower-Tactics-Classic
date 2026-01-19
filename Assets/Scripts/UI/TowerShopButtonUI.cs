using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopButtonUI : TooltipObject
{
    [Header("Cache")]
    [SerializeField] public TowerInfoUI towerInfo;
    [SerializeField] public Button shopButton;



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

    protected override void DisplayTooltip(GameObject _tooltip)
    {
        ShopTowerTooltipUI towerTooltip = _tooltip.GetComponent<ShopTowerTooltipUI>();
        if (towerTooltip != null)
        {
            towerTooltip.DisplayShopTowerData(towerInfo.TowerData);
        }
    }
}
