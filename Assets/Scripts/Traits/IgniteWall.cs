using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteWall : MonoBehaviour
{
    private readonly string BulletTag = "Bullet";

    [Header("Attributes")]
    [SerializeField] private int igniteDamage;
    [SerializeField] private float igniteTickSpeed;
    [SerializeField] private float igniteTickCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(BulletTag))
        {
            BulletBehavior bulletBehavior = collision.GetComponent<BulletBehavior>();
            if (bulletBehavior != null)
            {
                bulletBehavior.Ignite(igniteDamage, igniteTickSpeed, igniteTickCount);
            }
        }
    }
}
