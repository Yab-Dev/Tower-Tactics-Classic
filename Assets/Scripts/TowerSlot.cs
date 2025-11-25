using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TowerDragDrop currentTower;



    public void SetCurrentTower(TowerDragDrop tower)
    {
        currentTower = tower;
        currentTower.OnTowerMove += ClearTower;
    }

    public bool HasTower()
    {
        return currentTower != null;
    }

    public void ClearTower()
    {
        currentTower.OnTowerMove -= ClearTower;
        currentTower = null;
    }
}
