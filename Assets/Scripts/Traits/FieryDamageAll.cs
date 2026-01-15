using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieryDamageAll : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private float damageSpeed;

    private float damageTimer;
    private bool isDamaging;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += StartDamaging;
        GameManager.OnDefensePhaseEnd += StopDamaging;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= StartDamaging;
        GameManager.OnDefensePhaseEnd -= StopDamaging;
    }

    private void Update()
    {
        if (damageTimer <= 0.0f)
        {
            List<EnemyBehavior> enemies = WaveManager.GetInstance().GetEnemies();
            foreach (EnemyBehavior enemy in enemies)
            {
                enemy.Damage(damage);
            }
            damageTimer = damageSpeed;
        }
        else
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void StartDamaging(int _waveCount)
    {
        isDamaging = true;
        damageTimer = damageSpeed;
    }

    private void StopDamaging(int _waveCount)
    {
        isDamaging = false;
    }
}
