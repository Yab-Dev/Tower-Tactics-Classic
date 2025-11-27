using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopButtonUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TowerInfoUI towerInfo;
    [SerializeField] private Button shopButton;
    [SerializeField] private TowerData TESTDATA;



    private void Awake()
    {
        
    }

    private void Start()
    {
        towerInfo.SetTowerInfo(TESTDATA);
    }

    private void BuyTower()
    {

    }
}
