using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.SplashScreen;

public class TowerDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private readonly string TowerSlotTag = "TowerSlot";

    [Header("Attributes")]
    [SerializeField] private bool canDrag;
    [SerializeField] private bool startDragging;
    [SerializeField] private bool isDragging;
    [SerializeField] private GameObject currentDraggedSlot;
    [SerializeField] private GameObject startingSlot;

    public delegate void OnTowerMoveEventArgs();
    public event OnTowerMoveEventArgs OnTowerMove;

    public delegate void OnAnyTowerMoveEventArgs();
    public static event OnAnyTowerMoveEventArgs OnAnyTowerMoveStart;
    public static event OnAnyTowerMoveEventArgs OnAnyTowerMoveEnd;

    public delegate void OnTowerPlaceEventArgs();
    public static event OnTowerPlaceEventArgs OnTowerPlaceStart;
    public static event OnTowerPlaceEventArgs OnTowerPlaceEnd;

    private bool initialPlace = true;



    private void Awake()
    {
        GameManager.OnBuildPhaseStart += SetDraggable;
        GameManager.OnBuildPhaseEnd += SetUnDraggable;
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= SetDraggable;
        GameManager.OnBuildPhaseEnd -= SetUnDraggable;
    }

    private void Start()
    {
        if (startDragging)
        {
            StartDrag();
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            transform.position = mousePos;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!canDrag) { return; }
        if (!collision.CompareTag(TowerSlotTag)) { return; }
        if (!isDragging) { return; }

        float currentDistance = 0.0f;
        if (currentDraggedSlot == null)
        {
            currentDistance = float.MaxValue;
        }
        else
        {
            if (currentDraggedSlot == collision.gameObject)
            {
                return;
            }
            currentDistance = Vector2.Distance(transform.position, currentDraggedSlot.transform.position);
        }

        bool distanceCheck = Vector2.Distance(transform.position, collision.transform.position) < currentDistance;
        if (distanceCheck)
        {
            TowerSlot towerSlot = collision.GetComponent<TowerSlot>();
            if (towerSlot == null) { return; }

            if (!(startingSlot == null && towerSlot.HasTower()))
            {
                currentDraggedSlot = collision.gameObject;
            }
            else if (towerSlot.HasTower()) 
            {
                TowerBehavior slotBehavior = towerSlot.GetTower().GetComponent<TowerBehavior>();
                if (slotBehavior == null) { return; }
                TowerBehavior towerBehavior = GetComponent<TowerBehavior>();
                if (towerBehavior == null) { return; };

                if (slotBehavior.GetTowerData().name == towerBehavior.GetTowerData().name && slotBehavior.GetLevel() != slotBehavior.GetTowerData().stats.Count)
                {
                    currentDraggedSlot = collision.gameObject;
                }
            }
            return;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!canDrag) { return; }
        if (collision.CompareTag(TowerSlotTag) && isDragging)
        {
            if (currentDraggedSlot == collision.gameObject)
            {
                //currentDraggedSlot = null;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag) { return; }
        if (isDragging) { return; }

        initialPlace = false;
        StartDrag();
    }

    private void StartDrag()
    {
        canDrag = true;
        isDragging = true;
        startingSlot = null;
        OnTowerMove?.Invoke();
        OnAnyTowerMoveStart?.Invoke();
        if (initialPlace)
        {
            OnTowerPlaceStart?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canDrag) { return; }
        if (currentDraggedSlot != null && isDragging)
        {
            transform.position = currentDraggedSlot.transform.position;

            TowerSlot towerSlot = currentDraggedSlot.GetComponent<TowerSlot>();
            if (towerSlot == null) { return; }

            if (towerSlot.HasTower())
            {
                TowerBehavior slotTowerBehavior = towerSlot.GetTower().GetComponent<TowerBehavior>();
                if (slotTowerBehavior == null) { return; }
                TowerBehavior towerBehavior = GetComponent<TowerBehavior>();
                if (towerBehavior == null) { return; }
                
                if (slotTowerBehavior.GetTowerData().name == towerBehavior.GetTowerData().name && slotTowerBehavior.GetLevel() != slotTowerBehavior.GetTowerData().stats.Count)
                {
                    slotTowerBehavior.AddExp(towerBehavior.GetTotalExp());
                    OnAnyTowerMoveEnd?.Invoke();
                    if (initialPlace)
                    {
                        OnTowerPlaceEnd?.Invoke();
                    }
                    Destroy(gameObject);
                    return;
                }

                if (startingSlot != null)
                {
                    TowerSlot slot = startingSlot.GetComponent<TowerSlot>();
                    if (slot == null) { return; }
                    slot.ClearTower();
                    slot.SetCurrentTower(towerSlot.GetTower());
                    slot.GetTower().transform.position = startingSlot.transform.position;
                }
            }

            towerSlot.ClearTower();
            towerSlot.OnSlotTowerMoved += SetStartingSlot;
            towerSlot.SetCurrentTower(this);

            isDragging = false;
            OnAnyTowerMoveEnd?.Invoke();

            if (initialPlace)
            {
                OnTowerPlaceEnd?.Invoke();
                initialPlace = false;
            }
        }
    }

    private void SetDraggable(int _waveCount)
    {
        canDrag = true;
    }

    private void SetUnDraggable(int _waveCount)
    {
        canDrag = false;
    }

    public void SetStartingSlot(GameObject _slot)
    {
        startingSlot = _slot;
    }

    public void StartDraggable(bool _draggable)
    {
        startDragging = _draggable;
    }
}
