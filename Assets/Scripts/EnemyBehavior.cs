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

    public delegate void OnEnemyDestroyedEventArgs(Vector2 _deathPosition);
    public event OnEnemyDestroyedEventArgs OnEnemyDestroyed;
    public static event OnEnemyDestroyedEventArgs OnAnyEnemyDestroyed;


    private void Awake()
    {
        GameManager.OnClearEnemies += SelfDestruct;
        WaveManager.OnGetEnemies += FetchEnemyData;
    }

    private void OnDisable()
    {
        GameManager.OnClearEnemies -= SelfDestruct;
        WaveManager.OnGetEnemies -= FetchEnemyData;
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

    private void EnemyAttacking(GameObject _target)
    {
        if (attackCooldown <= 0.0f)
        {
            IDamage damageInterface = _target.GetComponent<IDamage>();
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

    private void SelfDestruct()
    {
        if (this == null) { return; }
        isDestroyed = true;
        OnEnemyDestroyed?.Invoke(transform.position);
        OnAnyEnemyDestroyed?.Invoke(transform.position);
        Destroy(gameObject);
    }

    private void FetchEnemyData(ref List<EnemyBehavior> _enemyData)
    {
        _enemyData.Add(this);
    }    

    public void Damage(int _amount)
    {
        currentHealth -= _amount;
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

    public void ApplyTags(List<BulletBehavior.BulletTags> _tags)
    {
        
    }

    public EnemyData EnemyData
    {
        private get { return enemyData; }
        set { enemyData = value; }
    }
}
