using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPush : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float growSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float knockbackAmount;
    [SerializeField] private float knockbackDuration;



    void Update()
    {
        transform.position += new Vector3(growSpeed / 2 * Time.deltaTime, 0);
        transform.localScale += new Vector3(growSpeed * Time.deltaTime, growSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage damageInterface = collision.GetComponent<IDamage>();
        if (damageInterface != null)
        {
            if (damageInterface.GetTeam() == IDamage.Team.Enemy)
            {
                damageInterface.Damage(damage);
                damageInterface.Knockback(knockbackDuration, knockbackAmount, false);
            }
        }
    }
}
