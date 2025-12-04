using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitTooltipUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text traitNameText;
    [SerializeField] private Image traitIcon;

    [Header("Prefabs")]
    [SerializeField] private GameObject traitBreakpointPrefab;

    private List<GameObject> traitBreakpointObjects = new List<GameObject>();



    public void DisplayTraitData(TraitData traitData, int traitCount)
    {
        foreach (GameObject traitBreakpointObject in traitBreakpointObjects)
        {
            Destroy(traitBreakpointObject);
        }
        traitBreakpointObjects.Clear();

        traitNameText.text = traitData.name;
        traitIcon.sprite = traitData.traitIcon;

        for (int i = 0; i < traitData.breakpoints.Count; i++)
        {
            GameObject traitBreakpointObject = Instantiate(traitBreakpointPrefab, transform);
            traitBreakpointObjects.Add(traitBreakpointObject);

            TraitBreakpointTooltipUI breakpointTooltipUI = traitBreakpointObject.GetComponent<TraitBreakpointTooltipUI>();
            if (breakpointTooltipUI != null)
            {
                breakpointTooltipUI.SetBreakpointData(traitData.breakpoints[i], traitCount);
            }
        }
    }
}
