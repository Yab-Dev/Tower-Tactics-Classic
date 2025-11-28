using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TowerData towerData;
    [SerializeField] private bool isFiring;

    [Header("Cache")]
    [SerializeField] SpriteRenderer sprite;

    private float shootCooldown;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += StartDefense;
        GameManager.OnDefensePhaseEnd += EndDefense;
    }

    private void Start()
    {
        sprite.sprite = towerData.sprite;
    }

    private void Update()
    {
        if (isFiring)
        {
            TowerFiring();
        }
    }

    private void TowerFiring()
    {
        if (shootCooldown <= 0.0f)
        {
            BulletBehavior.CreateBullet(towerData.bulletObject, transform.position, towerData.damage, towerData.bulletSpeed);
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
}
