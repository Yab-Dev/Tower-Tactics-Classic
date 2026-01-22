using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneBuff : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float buffInterval;
    [SerializeField] private int damageBuffAmount;
    [SerializeField] private float attackSpeedBuffAmount;

    [Header("Cache")]
    [SerializeField] private TraitData medievalTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject throneHornsPrefab;

    private float buffTimer;


    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += SelfDestruct;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= SelfDestruct;
    }

    private void Start()
    {
        buffTimer = buffInterval;
    }

    private void Update()
    {
        if (buffTimer < 0.0f)
        {
            List<TowerBehavior> medievalTowers = GameManager.GetInstance().GetTowersOfTrait(medievalTrait);
            foreach (TowerBehavior tower in medievalTowers)
            {
                float rand = Random.Range(0.0f, 1.0f);
                if (rand <= 0.5f)
                {
                    tower.AddStatModification(_damage: damageBuffAmount);
                }
                else
                {
                    tower.AddStatModification(_hitSpeed: attackSpeedBuffAmount);
                }
            }

            Instantiate(throneHornsPrefab);
            buffTimer = buffInterval;
        }
        else
        {
            buffTimer -= Time.deltaTime;
        }
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }
}
