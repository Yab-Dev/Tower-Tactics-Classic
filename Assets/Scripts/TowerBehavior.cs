using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] TowerData towerData;

    [Header("Cache")]
    [SerializeField] SpriteRenderer sprite;



    private void Start()
    {
        sprite.sprite = towerData.sprite;
    }
}
