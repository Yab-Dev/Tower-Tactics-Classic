using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitTooltipUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private TMPro.TMP_Text traitNameText;
    [SerializeField] private Image traitIcon;
    [SerializeField] private TMPro.TMP_Text traitDescriptionText;

    [Header("Prefabs")]
    [SerializeField] private GameObject traitBreakpointPrefab;

    private List<GameObject> traitBreakpointObjects = new List<GameObject>();



    public void DisplayTraitData(TraitData _traitData, int _traitCount)
    {
        foreach (GameObject traitBreakpointObject in traitBreakpointObjects)
        {
            Destroy(traitBreakpointObject);
        }
        traitBreakpointObjects.Clear();

        traitNameText.text = _traitData.name;
        traitIcon.sprite = _traitData.traitIcon;
        traitDescriptionText.text = _traitData.description;

        for (int i = 0; i < _traitData.breakpoints.Count; i++)
        {
            GameObject traitBreakpointObject = Instantiate(traitBreakpointPrefab, transform);
            traitBreakpointObjects.Add(traitBreakpointObject);

            TraitBreakpointTooltipUI breakpointTooltipUI = traitBreakpointObject.GetComponent<TraitBreakpointTooltipUI>();
            if (breakpointTooltipUI != null)
            {
                breakpointTooltipUI.SetBreakpointData(_traitData.breakpoints[i], _traitCount);
            }
        }
    }

    public void DisplayTraitData(TraitData _traitData)
    {
        DisplayTraitData(_traitData, -1);
    }
}
