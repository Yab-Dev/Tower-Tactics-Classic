using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLaser : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private IDamage.Team team;
    [SerializeField] private float animationSpeed;



    private void Start()
    {
        StartCoroutine(LaserScale());
    }

    private IEnumerator LaserScale()
    {
        Vector3 startScale = new Vector3(transform.localScale.x, 0.0f, transform.localScale.z);
        Vector3 endScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
        transform.localScale = startScale;

        float timeElapsed = 0.0f;

        while (timeElapsed < animationSpeed / 2.0f)
        {
            float t = timeElapsed / animationSpeed;

            transform.localScale = Vector3.Lerp(startScale, endScale, t);

            timeElapsed += Time.deltaTime;
            
            yield return null;
        }

        timeElapsed = 0.0f;
        while (timeElapsed < animationSpeed / 2.0f)
        {
            float t = timeElapsed / (animationSpeed / 2.0f);

            transform.localScale = Vector3.Lerp(endScale, startScale, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamage damageInterface = collision.GetComponent<IDamage>();
        if (damageInterface != null)
        {
            if (damageInterface.GetTeam() == team) { return; }

            damageInterface.Damage(damage);
        }
    }
}
