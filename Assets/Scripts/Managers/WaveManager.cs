using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance;

    [Header("Attributes")]
    [SerializeField] private Vector2 topLaneSpawnPos;
    [SerializeField] private Vector2 midLaneSpawnPos;
    [SerializeField] private Vector2 botLaneSpawnPos;
    [SerializeField] private LevelWaves levelWaves;

    private List<EnemyData> topEnemiesToSpawn = new List<EnemyData>();
    private List<EnemyData> midEnemiesToSpawn = new List<EnemyData>();
    private List<EnemyData> botEnemiesToSpawn = new List<EnemyData>();

    private int spawnedEnemies;

    public delegate void OnGetEnemiesEventArgs(ref List<EnemyBehavior> _enemies);
    public static event OnGetEnemiesEventArgs OnGetEnemies;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.OnDefensePhaseStart += StartSpawningWave;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= StartSpawningWave;
    }

    public static WaveManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("WaveManager Instance Not Found");
            return null;
        }

        return instance;
    }

    public LevelWaves.WaveData GetWaveData(int _waveCount)
    {
        return levelWaves.waves[_waveCount - 1];
    }

    private void StartSpawningWave(int _waveCount)
    {
        topEnemiesToSpawn.Clear();
        midEnemiesToSpawn.Clear();
        botEnemiesToSpawn.Clear();

        StartCoroutine(SpawnWave(levelWaves.waves[_waveCount - 1]));
    }

    private IEnumerator SpawnWave(LevelWaves.WaveData _waveData)
    {
        topEnemiesToSpawn.AddRange(_waveData.topLaneEnemies);
        midEnemiesToSpawn.AddRange(_waveData.midLaneEnemies);
        botEnemiesToSpawn.AddRange(_waveData.botLaneEnemies);

        while (topEnemiesToSpawn.Count > 0 || midEnemiesToSpawn.Count > 0 || botEnemiesToSpawn.Count > 0)
        {
            yield return new WaitForSeconds(_waveData.spawnDelay);

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
        yield return new WaitUntil(() => spawnedEnemies <= 0);

        GameManager.GetInstance().CompleteWave(levelWaves.waves.Count);
    }

    private void EnemyDestroyed(Vector2 _deathPosition)
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

    private EnemyData GetEnemyFromList(ref List<EnemyData> _list)
    {
        if (_list.Count == 0)
        {
            return null;
        }
        EnemyData enemyToSpawn = _list[0];
        _list.RemoveAt(0);
        return enemyToSpawn;
    }

    public List<EnemyBehavior> GetEnemies()
    {
        List<EnemyBehavior> enemyList = new List<EnemyBehavior>();

        OnGetEnemies?.Invoke(ref enemyList);

        return enemyList;
    }
}
