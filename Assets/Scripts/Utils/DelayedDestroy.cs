using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float destroyDelay;

    

    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
