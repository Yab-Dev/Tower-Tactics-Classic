using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTowerTooltipUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TowerTooltipUI towerTooltip;
    [SerializeField] private Transform traitTooltipContent;

    [Header("Prefabs")]
    [SerializeField] private GameObject traitTooltipPrefab;



    public void DisplayShopTowerData(TowerData _towerData)
    {
        towerTooltip.DisplayTowerData(_towerData);

        foreach (Transform child in traitTooltipContent)
        {
            Destroy(child.gameObject);
        }
        foreach (TraitData trait in _towerData.traits)
        {
            TraitTooltipUI traitTooltip = Instantiate(traitTooltipPrefab, traitTooltipContent).GetComponent<TraitTooltipUI>();
            if (traitTooltip != null)
            {
                traitTooltip.DisplayTraitData(trait);
            }
        }

        foreach (var layoutGroup in GetComponentsInChildren<LayoutGroup>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }
}
