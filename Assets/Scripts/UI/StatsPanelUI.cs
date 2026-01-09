using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color towerCapTextColor;
    [SerializeField] private Color invalidTowerCapTextColor;

    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text livesText;
    [SerializeField] private TMPro.TMP_Text towerTokensText;
    [SerializeField] private TMPro.TMP_Text towerCapText;



    private void Awake()
    {
        GameManager.OnLivesChanged += UpdateLives;
        ShopManager.OnTowerTokensChanged += UpdateTowerTokens;
        GameManager.OnCurrentTowersUpdated += UpdateTowerCap;
    }

    private void OnDisable()
    {
        GameManager.OnLivesChanged -= UpdateLives;
        ShopManager.OnTowerTokensChanged -= UpdateTowerTokens;
        GameManager.OnCurrentTowersUpdated -= UpdateTowerCap;
    }

    private void UpdateLives(int _currentLives)
    {
        livesText.text = _currentLives.ToString();
    }

    private void UpdateTowerTokens(int _towerTokenAmount)
    {
        towerTokensText.text = _towerTokenAmount.ToString();
    }

    private void UpdateTowerCap(List<GameObject> _towers, List<(TraitData trait, int count)> _traits, int _towerCap)
    {
        towerCapText.text = $"{_towers.Count}/{_towerCap}";

        if (_towers.Count > _towerCap)
        {
            towerCapText.color = invalidTowerCapTextColor;
        }
        else
        {
            towerCapText.color = towerCapTextColor;
        }
    }
}
