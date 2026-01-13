using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowInteractSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float spawningSpeed;

    [Header("Prefabs")]
    [SerializeField] private GameObject arrowInteractPrefab;

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
            Debug.Log("SPAWN");
            List<EnemyBehavior> enemyList = WaveManager.GetInstance().GetEnemies();
            Instantiate(arrowInteractPrefab, enemyList[Random.Range(0, enemyList.Count-1)].transform);

            spawnTimer = spawningSpeed;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void DestroySelf(int _waveCount)
    {
        Destroy(gameObject);
    }
}
