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
        TowerData towerData = _tower.GetTowerData();

        foreach (Transform child in towerTraitsContent)
        {
            Destroy(child.gameObject);
        }

        towerNameText.text = towerData.name;
        if (_tower.GetLevel() == towerData.stats.Count)
        {
            towerLevelText.text = $"Lv. Max";
        }
        else
        {
            towerLevelText.text = $"Lv. {_tower.GetLevel().ToString()}";

            for (int i = 0; i < _tower.GetExp().max; i++)
            {
                GameObject expObject = Instantiate(towerExpPrefab, towerExpContent);
                Image expImage = expObject.GetComponent<Image>();
                if (i < _tower.GetExp().current)
                {
                    expImage.color = expColor;
                }
            }
        }

        towerIcon.sprite = towerData.sprite;
        towerIcon.SetNativeSize();

        foreach (Transform child in towerTraitsContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < towerData.traits.Count; i++)
        {
            GameObject textObject = Instantiate(traitTextPrefab, towerTraitsContent);
            TMPro.TMP_Text traitText = textObject.GetComponent<TMPro.TMP_Text>();
            traitText.text = towerData.traits[i].name;
        }

        towerHealthText.text = $"Health: {_tower.GetCurrentHealth()}/{towerData.stats[_tower.GetLevel()-1].health}";
        towerHitspeedText.text = $"Hit Speed: {towerData.stats[_tower.GetLevel() - 1].hitSpeed.ToString()}";
        towerDamageText.text = $"Damage: {towerData.stats[_tower.GetLevel() - 1].damage.ToString()}";
        towerSellValueText.text = $"Sell Value: {_tower.GetSellValue()}";
        towerLaneRangeText.text = $"Lane Range: {towerData.stats[_tower.GetLevel() - 1].laneRange.ToString()}";
        towerAreaRangeText.text = $"Area Range: {towerData.stats[_tower.GetLevel() - 1].areaRange.ToString()}";
        towerHitCountText.text = "Hit Count: 1";

        towerDescriptionText.text = towerData.description;
    }
}
