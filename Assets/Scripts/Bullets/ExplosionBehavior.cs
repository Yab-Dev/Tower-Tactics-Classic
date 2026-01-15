using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private IDamage.Team team;
    [SerializeField] private int damage;
    [SerializeField] private List<BulletBehavior.BulletTags> tags = new List<BulletBehavior.BulletTags>();

    private List<GameObject> hasDamaged = new List<GameObject>();
    private BulletBehavior.IgniteData igniteData;


    public static void CreateExplosion(GameObject _explosionObject, Vector2 _position, IDamage.Team _team, int _damage, float _radius, List<BulletBehavior.BulletTags> _tags, BulletBehavior.IgniteData _igniteData)
    {
        GameObject explosion = Instantiate(_explosionObject, _position, Quaternion.identity);
        ExplosionBehavior explosionBehavior = explosion.GetComponent<ExplosionBehavior>();
        if (explosionBehavior != null)
        {
            explosionBehavior.team = _team;
            explosionBehavior.damage = _damage;
            explosion.transform.localScale = new Vector3(_radius, _radius, 0.0f);
            explosionBehavior.tags = _tags;
            explosionBehavior.igniteData = _igniteData;
        }
        else
        {
            Destroy(explosion);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDamaged.Contains(collision.gameObject)) { return; }

        IDamage damageInterface = collision.GetComponent<IDamage>();
        if (damageInterface != null)
        {
            if (damageInterface.GetTeam() == team) { return; }

            damageInterface.Damage(damage);
            damageInterface.ApplyTags(tags);
            if (igniteData.isIgnited)
            {
                damageInterface.Ignite(igniteData);
            }
            hasDamaged.Add(collision.gameObject);
        }
    }
}