using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, IDamage
{
    [Header("Attributes")]
    [SerializeField] private TowerData towerData;
    [SerializeField] private int currentHealth;
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
        RepairTower();
        targetDetection.SetSize(towerData.laneRange, towerData.areaRange);
    }

    private void Update()
    {
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
            BulletBehavior.CreateBullet(towerData.bulletObject, transform.position, target, IDamage.Team.Tower, towerData.damage, towerData.bulletSpeed);
            shootCooldown = towerData.hitSpeed;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    private void StartBuild()
    {
        RepairTower();
    }

    private void StartDefense(int waveCount)
    {
        shootCooldown = towerData.hitSpeed;
        isFiring = true;
    }

    private void EndDefense(int waveCount)
    {
        shootCooldown = towerData.hitSpeed;
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
        isDestroyed = false;
        towerCollision.enabled = true;
        currentHealth = towerData.health;
        sprite.sprite = towerData.sprite;
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
}
