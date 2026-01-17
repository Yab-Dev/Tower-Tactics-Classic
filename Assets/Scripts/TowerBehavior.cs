using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerBehavior : TooltipObject, IDamage
{
    [Header("Attributes")]
    [SerializeField] private TowerData towerData;
    [SerializeField] private Color hurtColor;
    [SerializeField] private int currentHealth;
    [SerializeField] private TowerData.LevelStats statModifier;
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExp;
    [SerializeField] private int maxExp;
    [SerializeField] private int totalExp = 1;
    [SerializeField] private int sellValue;
    [SerializeField] private bool isFiring;
    [SerializeField] private bool isDestroyed;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private BoxCollider2D towerCollision;
    [SerializeField] private TargetDetection targetDetection;
    [SerializeField] private ParticleSystem buffParticles;

    [Header("Prefabs")]
    [SerializeField] private GameObject healParticlesPrefab;

    public delegate void OnAnyTowerDestroyedEventArgs(TowerData _towerData, Vector2 _destroyPosition);
    public static event OnAnyTowerDestroyedEventArgs OnAnyTowerDestroyed;

    public delegate void OnAnyTowerHitEventArgs(TowerBehavior _towerBehavior);
    public static event OnAnyTowerHitEventArgs OnAnyTowerHit;

    private float shootCooldown;
    private Color originalColor;



    private void Awake()
    {
        GameManager.OnBuildPhaseStart += StartBuild;
        GameManager.OnDefensePhaseStart += StartDefense;
        GameManager.OnDefensePhaseEnd += EndDefense;

        GameManager.OnGetTowersOfTrait += FetchTowerTraitData;

        GameManager.OnClearStaticEffects += ClearStatModifications;
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= StartBuild;
        GameManager.OnDefensePhaseStart -= StartDefense;
        GameManager.OnDefensePhaseEnd -= EndDefense;

        GameManager.OnGetTowersOfTrait -= FetchTowerTraitData;

        GameManager.OnClearStaticEffects -= ClearStatModifications;
    }

    private void Start()
    {
        originalColor = sprite.color;
    }

    protected override void Update()
    {
        base.Update();

        if (isFiring && !isDestroyed)
        {
            GameObject target = targetDetection.GetClosestTarget(transform.position);
            if (target != null)
            {
                TowerFiring(target);
            }
        }
    }

    private void TowerFiring(GameObject _target)
    {
        if (CurrentStats.hitSpeed == 0) { return; }
        if (shootCooldown <= 0.0f)
        {
            StartCoroutine(ShootBullets(_target));
            shootCooldown = CurrentStats.hitSpeed;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    private IEnumerator ShootBullets(GameObject _target)
    {
        for (int i = 0; i < CurrentStats.hitCount; i++)
        {
            BulletBehavior.CreateBullet(towerData.bulletObject, transform.position, _target, IDamage.Team.Tower, CurrentStats.damage, towerData.bulletSpeed, _towerData: towerData);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void StartBuild(int _waveCount)
    {
        RepairTower();
    }

    private void StartDefense(int _waveCount)
    {
        shootCooldown = CurrentStats.hitSpeed;
        isFiring = true;
    }

    private void EndDefense(int _waveCount)
    {
        shootCooldown = CurrentStats.hitSpeed;
        isFiring = false;
        ClearStatModifications();
    }

    private void DestroyTower()
    {
        isDestroyed = true;
        towerCollision.enabled = false;
        sprite.sprite = towerData.destroyedSprite;
        OnAnyTowerDestroyed?.Invoke(towerData, transform.position);

        buffParticles.Stop();
    }

    private void RepairTower()
    {
        if (this != null)
        {
            isDestroyed = false;
            towerCollision.enabled = true;
            currentHealth = CurrentStats.health;
            sprite.sprite = towerData.sprite;
        }
    }

    public void SetStats()
    {
        sellValue = Mathf.FloorToInt(towerData.cost / 2.0f) * level;
        RepairTower();
        targetDetection.SetSize(CurrentStats.laneRange, CurrentStats.areaRange);
    }

    public void LevelUp()
    {
        level = Mathf.Min(level + 1, towerData.stats.Count);
        currentExp = 0;
        maxExp = 1 + level;
        SetStats();
    }

    public void LevelUp(int _towerLevel)
    {
        level = Mathf.Min(_towerLevel, towerData.stats.Count);
        currentExp = 0;
        maxExp = 1 + level;
        SetStats();
    }

    public void AddExp(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            currentExp++;
            totalExp++;
            if (currentExp >= maxExp)
            {
                LevelUp();
            }
        }
    }

    public void Damage(int _amount)
    {
        currentHealth -= _amount;
        if (currentHealth <= 0)
        {
            DestroyTower();
        }
        else
        {
            sprite.color = hurtColor;
            StartCoroutine(ColorFade.FadeSpriteColor(sprite, originalColor, 0.2f));
            OnAnyTowerHit?.Invoke(this);
        }
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Tower;
    }

    protected override void DisplayTooltip(GameObject _tooltip)
    {
        TowerTooltipUI towerTooltip = _tooltip.GetComponent<TowerTooltipUI>();
        if (towerTooltip != null)
        {
            towerTooltip.DisplayTowerData(this);
        }
    }

    public void ClearStatModifications()
    {
        currentHealth = Mathf.Max(1, currentHealth - statModifier.health);
        statModifier.health = 0;
        statModifier.hitSpeed = 1;
        statModifier.damage = 0;
        statModifier.laneRange = 0;
        statModifier.areaRange = 0;
        statModifier.hitCount = 0;

        buffParticles.Stop();
    }

    public void AddStatModification(int _health = 0, float _hitSpeed = 0, int _damage = 0, int _laneRange = 0, int _areaRange = 0, int _hitCount = 0)
    {
        statModifier.health += _health;
        statModifier.hitSpeed += _hitSpeed;
        statModifier.damage += _damage;
        statModifier.laneRange += _laneRange;
        statModifier.areaRange += _areaRange;
        statModifier.hitCount += _hitCount;

        buffParticles.Play();
    }

    private void FetchTowerTraitData(ref List<TowerBehavior> _towers, TraitData _trait)
    {
        if (isDestroyed) { return; }
        if (_trait == null || (_trait != null && towerData.traits.Contains(_trait)))
        {
            _towers.Add(this);
            return;
        }
    }

    public void HealTower(int _healAmount)
    {
        currentHealth += _healAmount;
        currentHealth = Mathf.Min(currentHealth, CurrentStats.health);
        Instantiate(healParticlesPrefab, transform);
    }

    public void ApplyTags(List<BulletBehavior.BulletTags> _tags)
    {
        
    }

    public void Ignite(BulletBehavior.IgniteData _igniteData)
    {
        
    }

    public void Slow(float _duration, float _amount)
    {
        
    }

    public void Knockback(float _duration, float _amount, bool toCenter)
    {
        
    }

    public TowerData TowerData
    {
        get { return towerData; }
        set { towerData = value; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int Level
    {
        get { return level; }
        private set { level = value; }
    }

    public (int current, int max) Exp
    {
        get { return (currentExp, maxExp); }
        private set 
        { 
            currentExp = value.current; 
            maxExp = value.max;
        }
    }

    public int TotalExp
    {
        get { return totalExp; }
        private set { totalExp = value; }
    }

    public int SellValue
    {
        get { return sellValue; }
        private set { sellValue = value; }
    }

    public TowerData.LevelStats CurrentStats
    {
        get { return towerData.stats[level-1] + statModifier; }
    }
}
