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
    [SerializeField] private TMPro.TMP_Text towerLaneRangeText;
    [SerializeField] private TMPro.TMP_Text towerAreaRangeText;
    [SerializeField] private TMPro.TMP_Text towerHitCountText;
    [SerializeField] private TMPro.TMP_Text towerDescriptionText;

    [Header("Prefabs")]
    [SerializeField] private GameObject towerExpPrefab;
    [SerializeField] private GameObject traitTextPrefab;



    public void DisplayTowerData(TowerBehavior tower)
    {
        TowerData towerData = tower.GetTowerData();

        towerNameText.text = towerData.name;
        towerLevelText.text = $"Lv. {tower.GetLevel().ToString()}";

        foreach (Transform child in towerTraitsContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < tower.GetExp().max; i++)
        {
            GameObject expObject = Instantiate(towerExpPrefab, towerExpContent);
            Image expImage = expObject.GetComponent<Image>();
            if (i < tower.GetExp().current)
            {
                expImage.color = expColor;
            }
        }

        towerIcon.sprite = towerData.sprite;

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

        towerHealthText.text = $"Health: {tower.GetCurrentHealth()}/{towerData.health}";
        towerHitspeedText.text = $"Hit Speed: {towerData.hitSpeed.ToString()}";
        towerDamageText.text = $"Damage: {towerData.damage.ToString()}";
        towerLaneRangeText.text = $"Lane Range: {towerData.laneRange.ToString()}";
        towerAreaRangeText.text = $"Area Range: {towerData.areaRange.ToString()}";
        towerHitCountText.text = "Hit Count: 1";

        towerDescriptionText.text = towerData.description;
    }
}
