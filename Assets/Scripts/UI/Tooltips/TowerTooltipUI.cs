using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerTooltipUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color expColor;

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
        if (_tower != null)
        {
            towerLevel = _tower.Level - 1;
        }

        string healthText = $"Health: {_towerData.stats[towerLevel].health}";
        if (_tower != null)
        {
            healthText = $"Health: {_tower.CurrentHealth}/{_towerData.stats[towerLevel].health}";
        }
        towerHealthText.text = healthText;

        towerHitspeedText.text = $"Hit Speed: {_towerData.stats[towerLevel].hitSpeed.ToString()}";
        towerDamageText.text = $"Damage: {_towerData.stats[towerLevel].damage.ToString()}";

        string sellValueText = $"Sell Value: N/A";
        if (_tower != null)
        {
            sellValueText = $"Sell Value: {_tower.SellValue}";
        }
        towerSellValueText.text = sellValueText;

        towerLaneRangeText.text = $"Lane Range: {_towerData.stats[towerLevel].laneRange.ToString()}";
        towerAreaRangeText.text = $"Area Range: {_towerData.stats[towerLevel].areaRange.ToString()}";
        towerHitCountText.text = "Hit Count: 1";

        towerDescriptionText.text = _towerData.description;
    }
}
