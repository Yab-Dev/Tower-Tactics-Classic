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
    [SerializeField] private ParticleSystem ignitedParticles;
    [SerializeField] private ParticleSystem slowedParticles;

    private float attackCooldown;
    private bool isDestroyed;

    private bool waitFrame = true;
    private Color originalColor;

    private BulletBehavior.IgniteData igniteData;

    private float slowDuration;
    private float slowAmount = 1.0f;

    private float knockbackDuration;
    private Vector2 knockbackPosition;

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

        ignitedParticles.Stop();
        slowedParticles.Stop();
    }

    private void Update()
    {
        if (knockbackDuration <= 0)
        {
            GameObject target = targetDetection.GetClosestTarget(transform.position);
            isAttacking = target != null;

            if (isAttacking)
            {
                EnemyAttacking(target);
            }
        }
        else
        {
            knockbackDuration -= Time.deltaTime;
        }

        if (slowDuration > 0.0f)
        {
            slowDuration -= Time.deltaTime;
            if (!slowedParticles.isPlaying)
            {
                slowedParticles.Play();
            }
        }
        else
        {
            slowAmount = 1.0f;
            slowedParticles.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (waitFrame)
        {
            waitFrame = false;
            return;
        }

        if (knockbackDuration <= 0.0f)
        {
            if (!isAttacking)
            {
                Vector3 moveTarget = transform.position - new Vector3(enemyData.moveSpeed * slowAmount * Time.fixedDeltaTime, 0.0f, 0.0f);
                rigidbody.MovePosition(moveTarget);
            }
        }
        else
        {
            Vector3 moveTarget = Vector3.MoveTowards(transform.position, knockbackPosition, (Vector3.Distance(transform.position, knockbackPosition) / 10.0f) * (Time.fixedDeltaTime * 40.0f));
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

    public void Ignite(BulletBehavior.IgniteData _igniteData)
    {
        igniteData = _igniteData;
        StopCoroutine(IgniteDamage());
        StartCoroutine(IgniteDamage());
    }

    private IEnumerator IgniteDamage()
    {
        ignitedParticles.Play();
        for (int i = 0; i < igniteData.igniteTickCount; i++)
        {
            Damage(igniteData.igniteDamage);
            yield return new WaitForSeconds(igniteData.igniteTickSpeed);
        }
        ignitedParticles.Stop();
    }

    public void Slow(float _duration, float _amount)
    {
        slowAmount = _amount;
        slowDuration = _duration;
    }

    public void Knockback(float _duration, float _amount, bool toCenter)
    {
        knockbackDuration = _duration;
        if (toCenter)
        {

        }
        else
        {
            targetDetection.ClearTargets();
            knockbackPosition = transform.position + new Vector3(_amount, 0);
        }
    }

    public EnemyData EnemyData
    {
        private get { return enemyData; }
        set { enemyData = value; }
    }
}
