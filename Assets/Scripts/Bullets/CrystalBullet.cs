using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CrystalBullet : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float pylonSpawnChance;
    [SerializeField] private float xDespawn;
    [SerializeField] private IDamage.Team team;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    [Header("Prefabs")]
    [SerializeField] private GameObject crystalPylonPrefab;



    public static void CreateCrystalBullet(GameObject _bulletObject, Vector2 _position, IDamage.Team _team, float _angle, float _speed, int _damage, float _lifetime)
    {
        if (_bulletObject == null) return;

        GameObject bullet = Instantiate(_bulletObject, _position, Quaternion.identity);
        CrystalBullet bulletBehavior = bullet.GetComponent<CrystalBullet>();
        if (bulletBehavior != null)
        {
            bulletBehavior.team = _team;
            bulletBehavior.damage = _damage;
            bulletBehavior.speed = _speed;
            bulletBehavior.lifetime = _lifetime;

            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        }
        else
        {
            Destroy(bullet);
        }
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f || transform.position.x > xDespawn)
        {
            PylonSpawn();

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

            PylonSpawn();

            Destroy(gameObject);
        }
    }

    private void PylonSpawn()
    {
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= pylonSpawnChance)
        {
            Instantiate(crystalPylonPrefab, transform.position, Quaternion.identity);
        }
    }
}
