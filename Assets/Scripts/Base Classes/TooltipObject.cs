using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TooltipObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("TooltipObject Prefabs")]
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private float tooltipDelay;

    private float tooltipTimer;
    private bool hovering = false;
    private bool displaying = false;


    protected virtual void Update()
    {
        if (hovering && tooltipPrefab != null)
        {
            tooltipTimer += Time.deltaTime;
            if (tooltipTimer >= tooltipDelay && displaying == false)
            {
                displaying = true;
                Time.timeScale = 0.2f;
                GameObject tooltip = GameManager.GetInstance().GetTooltipUI().SetTooltip(tooltipPrefab);
                DisplayTooltip(tooltip);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        displaying = false;
        tooltipTimer = 0.0f;
        Time.timeScale = 1.0f;

        GameManager.GetInstance().GetTooltipUI().ClearTooltip();
    }

    protected abstract void DisplayTooltip(GameObject tooltip);
}
