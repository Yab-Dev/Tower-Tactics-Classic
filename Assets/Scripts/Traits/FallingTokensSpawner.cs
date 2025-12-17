using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTokensSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Vector2 spawnXLevelBounds;
    [SerializeField] private float spawnYLevel;
    [SerializeField] private float spawningSpeed;

    [Header("Prefabs")]
    [SerializeField] private GameObject fallingTokenObject;

    private float spawnTimer;



    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += DestroySelf;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= DestroySelf;
    }

    private void Start()
    {
        spawnTimer = spawningSpeed;
    }

    private void Update()
    {
        if (spawnTimer <= 0.0f)
        {
            Instantiate(fallingTokenObject, new Vector3(Random.Range(spawnXLevelBounds.x, spawnXLevelBounds.y), spawnYLevel, 0.0f), Quaternion.identity);
            spawnTimer = spawningSpeed;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void DestroySelf(int waveCount)
    {
        Destroy(gameObject);
    }
}
