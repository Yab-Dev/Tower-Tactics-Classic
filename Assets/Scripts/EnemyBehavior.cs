using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamage
{
    [Header("Attributes")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isAttacking;
    [SerializeField] private Color hurtColor;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private TargetDetection targetDetection;

    private float attackCooldown;
    private bool isDestroyed;

    private bool waitFrame = true;
    private Color originalColor;

    public delegate void OnEnemyDestroyedEventArgs();
    public event OnEnemyDestroyedEventArgs OnEnemyDestroyed;


    private void Awake()
    {
        GameManager.OnClearEnemies += SelfDestruct;
    }

    private void OnDisable()
    {
        GameManager.OnClearEnemies -= SelfDestruct;
    }

    private void Start()
    {
        originalColor = sprite.color;

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
        if (waitFrame)
        {
            waitFrame = false;
            return;
        }

        if (!isAttacking)
        {
            Vector3 moveTarget = transform.position - new Vector3(enemyData.moveSpeed * Time.fixedDeltaTime, 0.0f, 0.0f);
            rigidbody.MovePosition(moveTarget);
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

    public void SetEnemyData(EnemyData data)
    {
        enemyData = data;
    }

    private void SelfDestruct()
    {
        if (this == null) { return; }
        isDestroyed = true;
        OnEnemyDestroyed?.Invoke();
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDestroyed)
        {
            SelfDestruct();
        }
        else
        {
            sprite.color = hurtColor;
            StartCoroutine(ColorFade.FadeSpriteColor(sprite, originalColor, 0.2f));
        }
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Enemy;
    }
}
