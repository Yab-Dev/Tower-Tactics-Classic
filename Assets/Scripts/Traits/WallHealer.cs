using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealer : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int healAmount;
    [SerializeField] private float healRate;

    [Header("Cache")]
    [SerializeField] private TraitData wallTrait;

    private float healTimer;


    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += DestroySelf;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= DestroySelf;
    }

    private void Start()
    {
        healTimer = healRate;
    }

    private void Update()
    {
        if (healTimer <= 0.0f)
        {
            List<TowerBehavior> wallTowers = GameManager.GetInstance().GetTowersOfTrait(wallTrait);
            foreach (TowerBehavior tower in wallTowers)
            {
                tower.HealTower(healAmount);
            }

            Debug.Log("HEAL");
            healTimer = healRate;
        }
        else
        {
            healTimer -= Time.deltaTime;
        }
    }

    private void DestroySelf(int _waveCount)
    {
        Destroy(gameObject);
    }
}
