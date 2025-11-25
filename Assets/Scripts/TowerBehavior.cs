using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] TowerData towerData;
    [SerializeField] bool isFiring;

    [Header("Cache")]
    [SerializeField] SpriteRenderer sprite;

    private float shootCooldown;



    private void Start()
    {
        sprite.sprite = towerData.sprite;
        shootCooldown = towerData.hitSpeed;
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
}
