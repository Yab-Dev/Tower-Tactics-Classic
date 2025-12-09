using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text towerTokensText;



    private void Awake()
    {
        ShopManager.OnTowerTokensChanged += UpdateTowerTokens;
    }

    private void UpdateTowerTokens(int towerTokenAmount)
    {
        towerTokensText.text = towerTokenAmount.ToString();
    }
}
