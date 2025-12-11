using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : TooltipObject, IDamage
{
    [Header("Attributes")]
    [SerializeField] private TowerData towerData;
    [SerializeField] private int currentHealth;
    [SerializeField] private int level;
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

    private float shootCooldown;



    private void Awake()
    {
        GameManager.OnBuildPhaseStart += StartBuild;
        GameManager.OnDefensePhaseStart += StartDefense;
        GameManager.OnDefensePhaseEnd += EndDefense;
    }

    private void Start()
    {
        LevelUp();
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

    private void TowerFiring(GameObject target)
    {
        if (shootCooldown <= 0.0f)
        {
            BulletBehavior.CreateBullet(towerData.bulletObject, transform.position, target, IDamage.Team.Tower, towerData.stats[level - 1].damage, towerData.bulletSpeed);
            shootCooldown = towerData.stats[level - 1].hitSpeed;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    private void StartBuild(int waveCount)
    {
        RepairTower();
    }

    private void StartDefense(int waveCount)
    {
        shootCooldown = towerData.stats[level - 1].hitSpeed;
        isFiring = true;
    }

    private void EndDefense(int waveCount)
    {
        shootCooldown = towerData.stats[level - 1].hitSpeed;
        isFiring = false;
    }

    public void SetTowerData(TowerData data)
    {
        towerData = data;
    }

    private void DestroyTower()
    {
        isDestroyed = true;
        towerCollision.enabled = false;
        sprite.sprite = towerData.destroyedSprite;
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

    public TowerData GetTowerData()
    {
        return towerData;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetLevel()
    {
        return level;
    }

    public (int current, int max) GetExp()
    {
        return (currentExp, maxExp);
    }

    public int GetTotalExp()
    {
        return totalExp;
    }

    public int GetSellValue()
    {
        return sellValue;
    }

    public void LevelUp()
    {
        level = Mathf.Min(level + 1, towerData.stats.Count);
        currentExp = 0;
        maxExp = 1 + level;
        sellValue = Mathf.FloorToInt(towerData.cost / 2.0f) * level;
        RepairTower();
        targetDetection.SetSize(towerData.stats[level - 1].laneRange, towerData.stats[level - 1].areaRange);
    }

    public void AddExp(int count)
    {
        for (int i = 0; i < count; i++)
        {
            currentExp++;
            totalExp++;
            if (currentExp >= maxExp)
            {
                LevelUp();
            }
        }
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Tower;
    }

    protected override void DisplayTooltip(GameObject tooltip)
    {
        TowerTooltipUI towerTooltip = tooltip.GetComponent<TowerTooltipUI>();
        if (towerTooltip != null)
        {
            towerTooltip.DisplayTowerData(this);
        }
    }
}
