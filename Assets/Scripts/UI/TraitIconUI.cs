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



    public void SetTraitData(TraitData _trait, int _count)
    {
        currentTrait = _trait;
        traitCount = _count;

        DisplayTraitData(_trait, _count);
    }

    protected override void DisplayTooltip(GameObject _tooltip)
    {
        TraitTooltipUI traitTooltipUI = _tooltip.GetComponent<TraitTooltipUI>();
        if (traitTooltipUI != null)
        {
            traitTooltipUI.DisplayTraitData(currentTrait, traitCount);
        }
    }

    private void DisplayTraitData(TraitData _trait, int _count)
    {
        traitImage.sprite = currentTrait.traitIcon;
        traitCountText.text = traitCount.ToString();

        for (int i = 0; i < _trait.breakpoints.Count; i++)
        {
            if (_count < _trait.breakpoints[i].breakpointValue)
            {
                backgroundImage.color = breakpointColors[i];
                return;
            }
        }
        backgroundImage.color = breakpointColors[_trait.breakpoints.Count];
    }
}
