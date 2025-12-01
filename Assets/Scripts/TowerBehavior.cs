using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, IDamage
{
    [Header("Attributes")]
    [SerializeField] private TowerData towerData;
    [SerializeField] private bool isFiring;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private TargetDetection targetDetection;

    private float shootCooldown;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += StartDefense;
        GameManager.OnDefensePhaseEnd += EndDefense;
    }

    private void Start()
    {
        sprite.sprite = towerData.sprite;
        targetDetection.SetSize(towerData.laneRange, towerData.areaRange);
    }

    private void Update()
    {
        if (isFiring)
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

    private void StartDefense()
    {
        shootCooldown = towerData.hitSpeed;
        isFiring = true;
    }

    private void EndDefense()
    {
        shootCooldown = towerData.hitSpeed;
        isFiring = false;
    }

    public void SetTowerData(TowerData data)
    {
        towerData = data;
    }

    public void Damage(int amount)
    {
        Debug.Log("Tower Damaged!");
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Tower;
    }
}
