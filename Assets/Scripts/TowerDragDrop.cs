using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private readonly string TowerSlotTag = "TowerSlot";

    [Header("Attributes")]
    [SerializeField] private bool canDrag;
    [SerializeField] private bool startDragging;
    [SerializeField] private bool isDragging;
    [SerializeField] private GameObject currentDraggedSlot;

    public delegate void OnTowerMoveEventArgs();
    public event OnTowerMoveEventArgs OnTowerMove;

    public delegate void OnAnyTowerMoveEventArgs();
    public static event OnAnyTowerMoveEventArgs OnAnyTowerMoveStart;
    public static event OnAnyTowerMoveEventArgs OnAnyTowerMoveEnd;



    private void Awake()
    {
        GameManager.OnBuildPhaseStart += SetDraggable;
        GameManager.OnBuildPhaseEnd += SetUnDraggable;
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
            if (towerSlot.HasTower()) { return; }

            currentDraggedSlot = collision.gameObject;
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

        StartDrag();
    }

    private void StartDrag()
    {
        canDrag = true;
        isDragging = true;
        OnTowerMove?.Invoke();
        OnAnyTowerMoveStart?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canDrag) { return; }
        if (currentDraggedSlot != null && isDragging)
        {
            transform.position = currentDraggedSlot.transform.position;

            TowerSlot towerSlot = currentDraggedSlot.GetComponent<TowerSlot>();
            if (towerSlot == null) { return; }
            towerSlot.SetCurrentTower(this);

            isDragging = false;
            OnAnyTowerMoveEnd?.Invoke();
        }
    }

    private void SetDraggable()
    {
        canDrag = true;
    }

    private void SetUnDraggable()
    {
        canDrag = false;
    }

    public void StartDraggable(bool draggable)
    {
        startDragging = draggable;
    }
}
