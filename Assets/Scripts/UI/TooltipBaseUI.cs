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
    [SerializeField] private RectTransform canvasTransform;

    public delegate void OnAssignTooltipObjectEventArgs(TooltipBaseUI _tooltipObject);
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

        //Debug.Log($"{Input.mousePosition} {rectTransform.position}");
        viewPortPos.x = Mathf.Max(0, Mathf.Sign(Input.mousePosition.x - (Screen.width / 2)));
        viewPortPos.y = Mathf.Max(0, Mathf.Sign(Input.mousePosition.y - (Screen.height / 2)));

        rectTransform.position = Input.mousePosition;
        rectTransform.anchoredPosition += new Vector2((viewPortPos.x * 2 - 1) * displayOffset.x, (viewPortPos.y * 2 - 1) * displayOffset.y);
        rectTransform.pivot = viewPortPos;
    }

    public GameObject SetTooltip(GameObject _tooltipPrefab)
    {
        ClearTooltip();

        GameObject tooltip = Instantiate(_tooltipPrefab, transform);
        dropShadowImage.enabled = true;
        Update();

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
