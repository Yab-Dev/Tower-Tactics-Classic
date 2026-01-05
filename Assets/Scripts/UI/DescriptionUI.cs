using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionUI : TooltipObject
{
    [Header("Attributes")]
    [SerializeField] [TextArea] private string description;

    protected override void DisplayTooltip(GameObject _tooltip)
    {
        DescriptionTooltipUI descriptionTooltip = _tooltip.GetComponent<DescriptionTooltipUI>();
        if (descriptionTooltip != null)
        {
            descriptionTooltip.DisplayDescription(description);
        }
    }
}
