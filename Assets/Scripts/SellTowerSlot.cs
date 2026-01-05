using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTowerSlot : TowerSlot
{
    public override void SetCurrentTower(TowerDragDrop _tower)
    {
        TowerBehavior towerBehavior = _tower.GetComponent<TowerBehavior>();
        if (towerBehavior != null)
        {
            ShopManager.GetInstance().TowerTokens += towerBehavior.SellValue;
            Destroy(_tower.gameObject);
            GameManager.GetInstance().GetTooltipUI().ClearTooltip();
        }
    }
}
