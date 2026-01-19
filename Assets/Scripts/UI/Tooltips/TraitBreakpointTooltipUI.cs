using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitBreakpointTooltipUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color breakpointReachedColor;
    [SerializeField] private Color breakpointNotReachedColor;

    [Header("Cache")]
    [SerializeField] private Image breakpointBackground;
    [SerializeField] private TMPro.TMP_Text breakpointCount;
    [SerializeField] private TMPro.TMP_Text breakpointDescription;



    public void SetBreakpointData(TraitData.BreakpointData breakpointData, int traitCount)
    {
        if (traitCount >= breakpointData.breakpointValue || traitCount == -1)
        {
            breakpointBackground.color = breakpointReachedColor;
        }
        else
        {
            breakpointBackground.color = breakpointNotReachedColor;
        }

        breakpointCount.text = breakpointData.breakpointValue.ToString();
        breakpointDescription.text = breakpointData.breakpointDescription.ToString();
    }
}
