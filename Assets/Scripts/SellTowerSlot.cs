using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTowerSlot : TowerSlot
{
    public override void SetCurrentTower(TowerDragDrop tower)
    {
        TowerBehavior towerBehavior = tower.GetComponent<TowerBehavior>();
        if (towerBehavior != null)
        {
            ShopManager.GetInstance().AddTowerTokens(towerBehavior.GetSellValue());
            Destroy(tower.gameObject);
        }
    }
}
