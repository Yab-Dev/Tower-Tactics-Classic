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


    public static void CreateBullet(GameObject bulletObject, Vector2 position, IDamage.Team team, int damage, float speed, float lifetime = 5.0f)
    {
        GameObject bullet = Instantiate(bulletObject, position, Quaternion.identity);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.team = team;
            bulletBehavior.damage = damage;
            bulletBehavior.speed = speed;
            bulletBehavior.lifetime = lifetime;
        }
        else
        {
            Destroy(bullet);
        }
    }

    private void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
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
}
