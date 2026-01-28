using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthInteractSpawner : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float spawningSpeed;
    [SerializeField] private EarthInteract.EarthMode earthMode;
    [SerializeField] private float knockbackInteractXPosition;
    [SerializeField] private List<float> laneYPositions;

    [Header("Prefabs")]
    [SerializeField] private GameObject earthInteractPrefab;

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
            switch(earthMode)
            {
                case EarthInteract.EarthMode.KnockbackLane:
                    Instantiate(earthInteractPrefab, new Vector3(knockbackInteractXPosition, laneYPositions[Random.Range(0, laneYPositions.Count)]), Quaternion.identity);
                    break;
                case EarthInteract.EarthMode.EnemyPush:
                    List<EnemyBehavior> enemies = WaveManager.GetInstance().GetEnemies();
                    List<EnemyBehavior> validEnemies = new List<EnemyBehavior>();
                    foreach(EnemyBehavior enemy in enemies)
                    {
                        if (enemy.CurrentLane != EnemyBehavior.LanePosition.Middle)
                        {
                            validEnemies.Add(enemy);
                        }
                    }
                    try
                    {
                        Transform enemyToSpawn = validEnemies[Random.Range(0, validEnemies.Count)].transform;
                        if (enemyToSpawn == null) { break; }
                        Instantiate(earthInteractPrefab, enemyToSpawn);
                    }
                    catch
                    { }
                    break;
            }

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
