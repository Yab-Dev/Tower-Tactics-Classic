using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Vector2 topLaneSpawnPos;
    [SerializeField] private Vector2 midLaneSpawnPos;
    [SerializeField] private Vector2 botLaneSpawnPos;
    [SerializeField] private LevelWaves levelWaves;

    private List<EnemyData> topEnemiesToSpawn = new List<EnemyData>();
    private List<EnemyData> midEnemiesToSpawn = new List<EnemyData>();
    private List<EnemyData> botEnemiesToSpawn = new List<EnemyData>();

    private int spawnedEnemies;


    private void Awake()
    {
        GameManager.OnDefensePhaseStart += StartSpawningWave;
    }

    private void StartSpawningWave(int waveCount)
    {
        topEnemiesToSpawn.Clear();
        midEnemiesToSpawn.Clear();
        botEnemiesToSpawn.Clear();

        StartCoroutine(SpawnWave(levelWaves.waves[waveCount-1]));
    }

    private IEnumerator SpawnWave(LevelWaves.WaveData waveData)
    {
        topEnemiesToSpawn.AddRange(waveData.topLaneEnemies);
        midEnemiesToSpawn.AddRange(waveData.midLaneEnemies);
        botEnemiesToSpawn.AddRange(waveData.botLaneEnemies);

        while (topEnemiesToSpawn.Count > 0 || midEnemiesToSpawn.Count > 0 || botEnemiesToSpawn.Count > 0)
        {
            yield return new WaitForSeconds(waveData.spawnDelay);

            var (enemyToSpawn, lane) = GetEnemyToSpawn();

            Vector2 spawnPos = Vector2.zero;
            switch (lane)
            {
                case 0: spawnPos = topLaneSpawnPos; break;
                case 1: spawnPos = midLaneSpawnPos; break;
                case 2: spawnPos = botLaneSpawnPos; break;
            }

            EnemyBehavior enemyBehavior = GameManager.GetInstance().SpawnEnemy(enemyToSpawn, spawnPos);
            enemyBehavior.OnEnemyDestroyed += EnemyDestroyed;
            spawnedEnemies++;
        }

        StartCoroutine(WaitUntilWaveOver());
    }

    private IEnumerator WaitUntilWaveOver()
    {
        yield return new WaitUntil(() => spawnedEnemies == 0);

        GameManager.GetInstance().CompleteWave(levelWaves.waves.Count);
    }

    private void EnemyDestroyed()
    {
        spawnedEnemies--;
    }

    private (EnemyData enemyData, int lane) GetEnemyToSpawn()
    {
        EnemyData enemyToSpawn = null;
        int laneToSpawn = 0;
        while (enemyToSpawn == null)
        {
            laneToSpawn = Random.Range(0, 3);
            switch (laneToSpawn)
            {
                case 0:
                    enemyToSpawn = GetEnemyFromList(ref topEnemiesToSpawn);
                    break;
                case 1:
                    enemyToSpawn = GetEnemyFromList(ref midEnemiesToSpawn);
                    break;
                case 2:
                    enemyToSpawn = GetEnemyFromList(ref botEnemiesToSpawn);
                    break;
            }
        }
        return (enemyToSpawn, laneToSpawn);
    }

    private EnemyData GetEnemyFromList(ref List<EnemyData> list)
    {
        if (list.Count == 0)
        {
            return null;
        }
        EnemyData enemyToSpawn = list[0];
        list.RemoveAt(0);
        return enemyToSpawn;
    }
}
