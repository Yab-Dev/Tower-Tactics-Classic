using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitIconUI : TooltipObject
{
    [Header("Attributes")]
    [SerializeField] private List<Color> breakpointColors = new List<Color>();
    [SerializeField] private TraitData currentTrait;
    [SerializeField] private int traitCount;

    [Header("Cache")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image traitImage;
    [SerializeField] private TMPro.TMP_Text traitCountText;



    public void SetTraitData(TraitData trait, int count)
    {
        currentTrait = trait;
        traitCount = count;

        DisplayTraitData(trait, count);
    }

    protected override void DisplayTooltip(GameObject tooltip)
    {
        TraitTooltipUI traitTooltipUI = tooltip.GetComponent<TraitTooltipUI>();
        if (traitTooltipUI != null)
        {
            traitTooltipUI.DisplayTraitData(currentTrait, traitCount);
        }
    }

    private void DisplayTraitData(TraitData trait, int count)
    {
        traitImage.sprite = currentTrait.traitIcon;
        traitCountText.text = traitCount.ToString();

        for (int i = 0; i < trait.breakpoints.Count; i++)
        {
            if (count < trait.breakpoints[i].breakpointValue)
            {
                backgroundImage.color = breakpointColors[i];
                return;
            }
        }
        backgroundImage.color = breakpointColors[trait.breakpoints.Count];
    }
}
