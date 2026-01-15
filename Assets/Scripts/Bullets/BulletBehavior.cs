using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private IDamage.Team team;
    [SerializeField] protected int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject target;
    [SerializeField] private List<BulletTags> tags = new List<BulletTags>();
    [SerializeField] private Color ignitedColor;

    [Header("Static Attributes")]
    [SerializeField] private float explosionBreakpoint2Radius;
    [SerializeField] private float explosionBreakpoint1Radius;
    [SerializeField] private float sniperBreakpoint1DamageScaling;

    [Header("Cache")]
    [SerializeField] private SpriteRenderer sprite;

    [Header("Prefabs")]
    [SerializeField] private GameObject explosionPrefab;

    public delegate void OnApplyBulletTagsEventArgs(ref List<BulletTags> _tags, TowerData _towerData);
    public static event OnApplyBulletTagsEventArgs OnApplyBulletTags;

    private float distanceTraveled;

    public struct IgniteData
    {
        public bool isIgnited;
        public int igniteDamage;
        public float igniteTickSpeed;
        public float igniteTickCount;
    }
    private IgniteData igniteData;



    public static void CreateBullet(GameObject _bulletObject, Vector2 _position, GameObject _target, IDamage.Team _team, int _damage, float _speed, float _lifetime = 5.0f, TowerData _towerData = null)
    {
        GameObject bullet = Instantiate(_bulletObject, _position, Quaternion.identity);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.team = _team;
            bulletBehavior.damage = _damage;
            bulletBehavior.speed = _speed;
            bulletBehavior.lifetime = _lifetime;
            bulletBehavior.target = _target;

            OnApplyBulletTags?.Invoke(ref bulletBehavior.tags, _towerData);
        }
        else
        {
            Destroy(bullet);
        }
    }

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
        if (target == null)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0.0f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        distanceTraveled += speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage damageInterface = collision.GetComponent<IDamage>();
        if (damageInterface != null)
        {
            if (damageInterface.GetTeam() == team) { return; }

            OnHit(collision, damageInterface);
            Destroy(gameObject);
        }
    }

    protected virtual void OnHit(Collider2D _collision, IDamage _damageInterface)
    {
        if (tags.Contains(BulletTags.Sniper))
        {
            float scalar = 1 + distanceTraveled * sniperBreakpoint1DamageScaling;
            damage = Mathf.RoundToInt(damage * scalar);
        }

        if (tags.Contains(BulletTags.Explosive2))
        {
            ExplosionBehavior.CreateExplosion(explosionPrefab, transform.position, IDamage.Team.Tower, damage, explosionBreakpoint2Radius, tags, igniteData);
        }
        else if (tags.Contains(BulletTags.Explosive1))
        {
            ExplosionBehavior.CreateExplosion(explosionPrefab, transform.position, IDamage.Team.Tower, damage, explosionBreakpoint1Radius, tags, igniteData);
        }
        else
        {
            _damageInterface.Damage(damage);
            _damageInterface.ApplyTags(tags);
            if (igniteData.isIgnited)
            {
                _damageInterface.Ignite(igniteData);
            }
        }
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }

    public void Ignite(int _igniteDamage, float _igniteTickSpeed, float _igniteTickCount)
    {
        tags.Add(BulletTags.Ignited);
        igniteData.isIgnited = true;
        igniteData.igniteDamage = _igniteDamage;
        igniteData.igniteTickSpeed = _igniteTickSpeed;
        igniteData.igniteTickCount = _igniteTickCount;

        sprite.color = ignitedColor;
    }

    public enum BulletTags { Explosive1, Explosive2, Sniper, Earthy, Ignited }
}
