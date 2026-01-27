using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IDamage;

public class PterodactylBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float followRadius;
    [SerializeField] private float radiusRetargetCooldown;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int attackDamage;

    [Header("Cache")]
    [SerializeField] private TargetDetection targetDetection;
    [SerializeField] private CircleCollider2D collider;

    private Vector3 targetPosition;
    private float retargetCooldown;
    private float attackTimer;
    private bool attacking = false;


    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += SelfDestruct;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= SelfDestruct;
    }

    private void Update()
    {
        if (targetDetection.GetTargets().Count > 0 && attackTimer <= 0.0f)
        {
            attackTimer = attackCooldown;
            attacking = true;
        }

        if (attacking)
        {
            if (targetDetection.GetTargets().Count == 0)
            {
                attacking = false;
            }
            AttackEnemy();
        }
        else
        {
            FollowCursor();
            attackTimer -= Time.deltaTime;
        }

        collider.enabled = attacking;
    }

    private void FollowCursor()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0.0f;

        float mouseDistance = Vector3.Distance(transform.position, worldMousePos);
        if (mouseDistance <= followRadius && retargetCooldown <= 0.0f)
        {
            targetPosition = worldMousePos + new Vector3(Random.Range(-followRadius, followRadius), Random.Range(-followRadius, followRadius));
            retargetCooldown = radiusRetargetCooldown;
        }
        else if (retargetCooldown <= 0.0f)
        {
            targetPosition = worldMousePos;
        }
        retargetCooldown -= Time.deltaTime;

        Vector3 moveVector = Vector3.MoveTowards(transform.position, targetPosition, Vector3.Distance(transform.position, targetPosition) * followSpeed * Time.deltaTime);

        if (transform.position.x < moveVector.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position = moveVector;
    }

    private void AttackEnemy()
    {
        GameObject target = targetDetection.GetClosestTarget(transform.position);

        Vector3 moveVector = Vector3.MoveTowards(transform.position, target.transform.position, targetSpeed * Time.deltaTime);

        if (transform.position.x < moveVector.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position = moveVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage damageInterface = collision.GetComponent<IDamage>();
        if (damageInterface != null)
        {
            if (damageInterface.GetTeam() == Team.Tower) { return; }

            damageInterface.Damage(attackDamage);
            attacking = false;
        }
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }
}
