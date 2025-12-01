using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamage
{
    [Header("Attributes")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isAttacking;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private TargetDetection targetDetection;

    private float attackCooldown;



    private void Awake()
    {
        
    }

    private void Start()
    {
        sprite.sprite = enemyData.sprite;
        currentHealth = enemyData.health;
        targetDetection.SetSize(enemyData.range, 0.5f);
        attackCooldown = enemyData.hitSpeed;
    }

    private void Update()
    {
        GameObject target = targetDetection.GetClosestTarget(transform.position);
        isAttacking = target != null;

        if (isAttacking)
        {
            EnemyAttacking(target);
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rigidbody.MovePosition(transform.position - new Vector3(enemyData.moveSpeed * Time.fixedDeltaTime, 0.0f, 0.0f));
        }
    }

    private void EnemyAttacking(GameObject target)
    {
        if (attackCooldown <= 0.0f)
        {
            IDamage damageInterface = target.GetComponent<IDamage>();
            if (damageInterface != null)
            {
                damageInterface.Damage(enemyData.damage);
            }
            attackCooldown = enemyData.hitSpeed;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Enemy;
    }
}
