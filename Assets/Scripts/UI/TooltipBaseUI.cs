using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipBaseUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Vector2 displayOffset;

    [Header("Cache")]
    [SerializeField] private Image dropShadowImage;

    public delegate void OnAssignTooltipObjectEventArgs(TooltipBaseUI tooltipObject);
    public static event OnAssignTooltipObjectEventArgs OnAssignTooltipObject;

    private RectTransform rectTransform;



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        OnAssignTooltipObject?.Invoke(this);
    }

    private void Update()
    {
        Vector3 viewPortPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        viewPortPos.x = Mathf.Floor(viewPortPos.x + 0.5f);
        viewPortPos.y = Mathf.Floor(viewPortPos.y + 0.5f);

        rectTransform.anchoredPosition = Input.mousePosition;
        rectTransform.anchoredPosition += new Vector2((viewPortPos.x * 2 - 1) * displayOffset.x, (viewPortPos.y * 2 - 1) * displayOffset.y);
        rectTransform.pivot = viewPortPos;
    }

    public GameObject SetTooltip(GameObject tooltipPrefab)
    {
        ClearTooltip();

        GameObject tooltip = Instantiate(tooltipPrefab, transform);
        dropShadowImage.enabled = true;

        return tooltip;
    }

    public void ClearTooltip()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        dropShadowImage.enabled = false;
    }
}
