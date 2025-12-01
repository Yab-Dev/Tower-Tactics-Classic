using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Waves", menuName = "Level Waves")]
public class LevelWaves : ScriptableObject
{
    public List<WaveData> waves;

    [System.Serializable]
    public class WaveData
    {
        public float spawnDelay;

        public bool topLaneHazard;
        public bool midLaneHazard;
        public bool botLaneHazard;

        public List<EnemyData> topLaneEnemies;
        public List<EnemyData> midLaneEnemies;
        public List<EnemyData> botLaneEnemies;
    }
}