using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private IDamage.Team team;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject target;


    public static void CreateBullet(GameObject _bulletObject, Vector2 _position, GameObject _target, IDamage.Team _team, int _damage, float _speed, float _lifetime = 5.0f)
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

            damageInterface.Damage(damage);
            Destroy(gameObject);
        }
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }
}
