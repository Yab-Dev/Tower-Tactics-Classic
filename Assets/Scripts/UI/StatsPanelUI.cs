using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color towerCapTextColor;
    [SerializeField] private Color invalidTowerCapTextColor;

    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text towerTokensText;
    [SerializeField] private TMPro.TMP_Text towerCapText;



    private void Awake()
    {
        ShopManager.OnTowerTokensChanged += UpdateTowerTokens;
        GameManager.OnCurrentTowersUpdated += UpdateTowerCap;
    }

    private void OnDisable()
    {
        ShopManager.OnTowerTokensChanged -= UpdateTowerTokens;
        GameManager.OnCurrentTowersUpdated -= UpdateTowerCap;
    }

    private void UpdateTowerTokens(int towerTokenAmount)
    {
        towerTokensText.text = towerTokenAmount.ToString();
    }

    private void UpdateTowerCap(List<GameObject> towers, List<(TraitData trait, int count)> traits, int towerCap)
    {
        towerCapText.text = $"{towers.Count}/{towerCap}";

        if (towers.Count > towerCap)
        {
            towerCapText.color = invalidTowerCapTextColor;
        }
        else
        {
            towerCapText.color = towerCapTextColor;
        }
    }
}
