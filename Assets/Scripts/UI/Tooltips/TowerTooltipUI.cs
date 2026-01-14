using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerTooltipUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color expColor;
    [SerializeField] private Color statTextColor;
    [SerializeField] private Color buffedStatTextColor;

    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text towerNameText;
    [SerializeField] private TMPro.TMP_Text towerLevelText;
    [SerializeField] private Transform towerExpContent;
    [SerializeField] private Image towerIcon;
    [SerializeField] private Transform towerTraitsContent;
    [SerializeField] private TMPro.TMP_Text towerHealthText;
    [SerializeField] private TMPro.TMP_Text towerHitspeedText;
    [SerializeField] private TMPro.TMP_Text towerDamageText;
    [SerializeField] private TMPro.TMP_Text towerSellValueText;
    [SerializeField] private TMPro.TMP_Text towerLaneRangeText;
    [SerializeField] private TMPro.TMP_Text towerAreaRangeText;
    [SerializeField] private TMPro.TMP_Text towerHitCountText;
    [SerializeField] private TMPro.TMP_Text towerDescriptionText;

    [Header("Prefabs")]
    [SerializeField] private GameObject towerExpPrefab;
    [SerializeField] private GameObject traitTextPrefab;



    public void DisplayTowerData(TowerBehavior _tower)
    {
        DisplayTowerData(_tower, _tower.TowerData);
    }

    public void DisplayTowerData(TowerData towerData)
    {
        DisplayTowerData(null, towerData);
    }

    private void DisplayTowerData(TowerBehavior _tower,  TowerData _towerData)
    {
        foreach (Transform child in towerExpContent)
        {
            Destroy(child.gameObject);
        }

        towerNameText.text = _towerData.name;

        if (_tower != null)
        {
            if (_tower.Level == _towerData.stats.Count)
            {
                towerLevelText.text = $"Lv. Max";
            }
            else
            {
                towerLevelText.text = $"Lv. {_tower.Level.ToString()}";

                for (int i = 0; i < _tower.Exp.max; i++)
                {
                    GameObject expObject = Instantiate(towerExpPrefab, towerExpContent);
                    Image expImage = expObject.GetComponent<Image>();
                    if (i < _tower.Exp.current)
                    {
                        expImage.color = expColor;
                    }
                }
            }
        }
        else
        {
            towerLevelText.text = "";
        }

        towerIcon.sprite = _towerData.sprite;
        towerIcon.SetNativeSize();

        foreach (Transform child in towerTraitsContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < _towerData.traits.Count; i++)
        {
            GameObject textObject = Instantiate(traitTextPrefab, towerTraitsContent);
            TMPro.TMP_Text traitText = textObject.GetComponent<TMPro.TMP_Text>();
            traitText.text = _towerData.traits[i].name;
        }

        int towerLevel = 0;
        TowerData.LevelStats towerStats;
        TowerData.LevelStats towerBaseStats;
        if (_tower != null)
        {
            towerLevel = _tower.Level - 1;
            towerStats = _tower.CurrentStats;
            towerBaseStats = _tower.TowerData.stats[towerLevel];
        }
        else
        {
            towerStats = _towerData.stats[towerLevel];
            towerBaseStats = towerStats;
        }

        string healthText = $"Health: {towerStats.health}";
        if (_tower != null)
        {
            healthText = $"Health: {_tower.CurrentHealth}/{towerStats.health}";
        }
        towerHealthText.text = healthText;
        ChangeStatTextColor(ref towerHealthText, towerStats.health != towerBaseStats.health);

        towerHitspeedText.text = $"Hit Speed: {towerStats.hitSpeed.ToString()}";
        ChangeStatTextColor(ref towerHitspeedText, towerStats.hitSpeed != towerBaseStats.hitSpeed);
        towerDamageText.text = $"Damage: {towerStats.damage.ToString()}";
        ChangeStatTextColor(ref towerDamageText, towerStats.damage != towerBaseStats.damage);

        string sellValueText = $"Sell Value: N/A";
        if (_tower != null)
        {
            sellValueText = $"Sell Value: {_tower.SellValue}";
        }
        towerSellValueText.text = sellValueText;
        
        towerLaneRangeText.text = $"Lane Range: {towerStats.laneRange.ToString()}";
        ChangeStatTextColor(ref towerLaneRangeText, towerStats.laneRange != towerBaseStats.laneRange);
        towerAreaRangeText.text = $"Area Range: {towerStats.areaRange.ToString()}";
        ChangeStatTextColor(ref towerAreaRangeText, towerStats.areaRange != towerBaseStats.areaRange);
        towerHitCountText.text = $"Hit Count: {towerStats.hitCount.ToString()}";
        ChangeStatTextColor(ref towerHitCountText, towerStats.hitCount != towerBaseStats.hitCount);

        towerDescriptionText.text = _towerData.description;
    }

    private void ChangeStatTextColor(ref TMPro.TMP_Text _textElement, bool _isStatBuffed)
    {
        if (_isStatBuffed)
        {
            _textElement.color = buffedStatTextColor;
        }
        else
        {
            _textElement.color = statTextColor;
        }
    }
}
