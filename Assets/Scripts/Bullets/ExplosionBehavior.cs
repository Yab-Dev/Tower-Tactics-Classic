using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletBehavior;

public class ExplosionBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private IDamage.Team team;
    [SerializeField] private int damage;
    [SerializeField] private List<BulletBehavior.BulletTags> tags = new List<BulletBehavior.BulletTags>();

    [Header("Prefabs")]
    [SerializeField] private GameObject crystalBulletPrefab;

    private List<GameObject> hasDamaged = new List<GameObject>();
    private BulletBehavior.IgniteData igniteData;

    private float slowDuration;
    private float slowAmount;


    public static void CreateExplosion(GameObject _explosionObject, Vector2 _position, IDamage.Team _team, int _damage, float _radius, List<BulletBehavior.BulletTags> _tags, BulletBehavior.IgniteData _igniteData, float _slowDuration, float _slowAmount)
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
            explosionBehavior.slowDuration = _slowDuration;
            explosionBehavior.slowAmount = _slowAmount;
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

            damageInterface.ApplyTags(tags);
            if (igniteData.isIgnited)
            {
                damageInterface.Ignite(igniteData);
            }
            if (tags.Contains(BulletTags.Icy))
            {
                damageInterface.Slow(slowDuration, slowAmount);
            }
            if (tags.Contains(BulletTags.Crystalline))
            {
                CrystalBullet.CreateCrystalBullet(crystalBulletPrefab, transform.position + new Vector3(1.5f, 0), team, 0.0f, 5.0f, Mathf.RoundToInt(damage / 4.0f), 0.5f);
                CrystalBullet.CreateCrystalBullet(crystalBulletPrefab, transform.position + new Vector3(1.5f, 1), team, 30.0f, 5.0f, Mathf.RoundToInt(damage / 4.0f), 0.5f);
                CrystalBullet.CreateCrystalBullet(crystalBulletPrefab, transform.position + new Vector3(1.5f, -1), team, -30.0f, 5.0f, Mathf.RoundToInt(damage / 4.0f), 0.5f);
            }
            hasDamaged.Add(collision.gameObject);
            damageInterface.Damage(damage);
        }
    }
}