using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBulletBehavior : BulletBehavior
{
    [Header("Attributes")]
    [SerializeField] private float explosionBreakpoint1Radius;
    [SerializeField] private float explosionBreakpoint2Radius;

    [Header("Cache")]
    [SerializeField] private TraitData explosiveTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject explosionPrefab;



    protected override void OnHit(Collider2D _collision, IDamage _damageInterface)
    {
        if (TraitUtils.CheckTraitBreakpoint(explosiveTrait, 1))
        {
            ExplosionBehavior.CreateExplosion(explosionPrefab, transform.position, IDamage.Team.Tower, damage, explosionBreakpoint2Radius);
        }
        else if (TraitUtils.CheckTraitBreakpoint(explosiveTrait, 0))
        {
            ExplosionBehavior.CreateExplosion(explosionPrefab, transform.position, IDamage.Team.Tower, damage, explosionBreakpoint1Radius);
        }
        else
        {
            base.OnHit(_collision, _damageInterface);
        }
    }
}
