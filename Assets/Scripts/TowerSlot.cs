using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TowerDragDrop currentTower;
    [SerializeField] private Color fadeInColor;
    [SerializeField] private Color fadeOutColor;
    [SerializeField] private bool isRearSlot;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;

    public delegate void OnSlotTowerMovedEventArgs(GameObject _slot);
    public event OnSlotTowerMovedEventArgs OnSlotTowerMoved;


    private void Awake()
    {
        TowerDragDrop.OnAnyTowerMoveStart += FadeSlotIn;
        TowerDragDrop.OnAnyTowerMoveEnd += FadeSlotOut;

        GameManager.OnGetPlacedTowers += GetCurrentTower;
    }

    private void OnDisable()
    {
        TowerDragDrop.OnAnyTowerMoveStart -= FadeSlotIn;
        TowerDragDrop.OnAnyTowerMoveEnd -= FadeSlotOut;

        GameManager.OnGetPlacedTowers -= GetCurrentTower;
        if (currentTower != null)
        {
            currentTower.OnTowerMove -= ClearTower;
        }
    }

    private void Start()
    {
        sprite.color = fadeOutColor;
    }

    public virtual void SetCurrentTower(TowerDragDrop _tower)
    {
        currentTower = _tower;
        currentTower.IsInRearSlot = isRearSlot;
        currentTower.OnTowerMove += ClearTower;
    }

    public bool HasTower()
    {
        return currentTower != null;
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

    private void GetCurrentTower(ref List<GameObject> _towers)
    {
        if (currentTower != null)
        {
            _towers.Add(currentTower.gameObject);
        }
    }

    public TowerDragDrop CurrentTower
    {
        get { return currentTower; }
        private set { currentTower = value; }
    }
}
