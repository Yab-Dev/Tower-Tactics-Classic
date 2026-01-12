using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : TooltipObject, IDamage
{
    [Header("Attributes")]
    [SerializeField] private TowerData towerData;
    [SerializeField] private Color hurtColor;
    [SerializeField] private int currentHealth;
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

    public delegate void OnAnyTowerDestroyedEventArgs(TowerData _towerData, Vector2 _destroyPosition);
    public static event OnAnyTowerDestroyedEventArgs OnAnyTowerDestroyed;

    private float shootCooldown;
    private Color originalColor;



    private void Awake()
    {
        GameManager.OnBuildPhaseStart += StartBuild;
        GameManager.OnDefensePhaseStart += StartDefense;
        GameManager.OnDefensePhaseEnd += EndDefense;
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= StartBuild;
        GameManager.OnDefensePhaseStart -= StartDefense;
        GameManager.OnDefensePhaseEnd -= EndDefense;
    }

    private void Start()
    {
        originalColor = sprite.color;
        LevelUp(1);
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
        if (shootCooldown <= 0.0f)
        {
            BulletBehavior.CreateBullet(towerData.bulletObject, transform.position, _target, IDamage.Team.Tower, towerData.stats[level - 1].damage, towerData.bulletSpeed);
            shootCooldown = towerData.stats[level - 1].hitSpeed;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    private void StartBuild(int _waveCount)
    {
        RepairTower();
    }

    private void StartDefense(int _waveCount)
    {
        shootCooldown = towerData.stats[level - 1].hitSpeed;
        isFiring = true;
    }

    private void EndDefense(int _waveCount)
    {
        shootCooldown = towerData.stats[level - 1].hitSpeed;
        isFiring = false;
    }

    private void DestroyTower()
    {
        isDestroyed = true;
        towerCollision.enabled = false;
        sprite.sprite = towerData.destroyedSprite;
        OnAnyTowerDestroyed?.Invoke(towerData, transform.position);
    }

    private void RepairTower()
    {
        if (this != null)
        {
            isDestroyed = false;
            towerCollision.enabled = true;
            currentHealth = towerData.stats[level - 1].health;
            sprite.sprite = towerData.sprite;
        }
    }

    public void SetStats()
    {
        sellValue = Mathf.FloorToInt(towerData.cost / 2.0f) * level;
        RepairTower();
        targetDetection.SetSize(towerData.stats[level - 1].laneRange, towerData.stats[level - 1].areaRange);
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

    public TowerData TowerData
    {
        get { return towerData; }
        set { towerData = value; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        private set { currentHealth = value; }
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
}
