using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TooltipObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("TooltipObject Prefabs")]
    [SerializeField] private GameObject tooltipPrefab;



    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject tooltip = GameManager.GetInstance().GetTooltipUI().SetTooltip(tooltipPrefab);
        DisplayTooltip(tooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.GetInstance().GetTooltipUI().ClearTooltip();
    }

    protected abstract void DisplayTooltip(GameObject tooltip);
}
