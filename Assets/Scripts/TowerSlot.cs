using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TowerDragDrop currentTower;
    [SerializeField] private Color fadeInColor;
    [SerializeField] private Color fadeOutColor;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;

    public delegate void OnSlotTowerMovedEventArgs(GameObject slot);
    public event OnSlotTowerMovedEventArgs OnSlotTowerMoved;


    private void Awake()
    {
        TowerDragDrop.OnAnyTowerMoveStart += FadeSlotIn;
        TowerDragDrop.OnAnyTowerMoveEnd += FadeSlotOut;

        GameManager.OnGetPlacedTowers += GetCurrentTower;
    }

    private void Start()
    {
        sprite.color = fadeOutColor;
    }

    public void SetCurrentTower(TowerDragDrop tower)
    {
        currentTower = tower;
        currentTower.OnTowerMove += ClearTower;
    }

    public bool HasTower()
    {
        return currentTower != null;
    }

    public TowerDragDrop GetTower()
    {
        return currentTower;
    }

    public void ClearTower()
    {
        if (currentTower != null)
        {
            currentTower.OnTowerMove -= ClearTower;
        }
        OnSlotTowerMoved?.Invoke(gameObject);
        currentTower = null;
    }

    private void FadeSlotIn()
    {
        StartCoroutine(ColorFade.FadeSpriteColor(sprite, fadeInColor, 0.2f));
    }

    private void FadeSlotOut()
    {
        StartCoroutine(ColorFade.FadeSpriteColor(sprite, fadeOutColor, 0.2f));
    }

    private void GetCurrentTower(ref List<GameObject> towers)
    {
        if (currentTower != null)
        {
            towers.Add(currentTower.gameObject);
        }
    }
}
