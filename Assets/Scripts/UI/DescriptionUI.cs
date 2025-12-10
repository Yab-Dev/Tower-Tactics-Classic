using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionUI : TooltipObject
{
    [Header("Attributes")]
    [SerializeField] [TextArea] private string description;

    protected override void DisplayTooltip(GameObject tooltip)
    {
        DescriptionTooltipUI descriptionTooltip = tooltip.GetComponent<DescriptionTooltipUI>();
        if (descriptionTooltip != null)
        {
            descriptionTooltip.DisplayDescription(description);
        }
    }
}
