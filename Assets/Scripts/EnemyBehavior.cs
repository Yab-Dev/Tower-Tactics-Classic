using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamage
{
    [Header("Attributes")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private int currentHealth;

    [Header("Cache")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] private new Rigidbody2D rigidbody;



    private void Awake()
    {
        
    }

    private void Start()
    {
        sprite.sprite = enemyData.sprite;
        currentHealth = enemyData.health;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position - new Vector3(enemyData.moveSpeed * Time.fixedDeltaTime, 0.0f, 0.0f));
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Enemy;
    }
}
