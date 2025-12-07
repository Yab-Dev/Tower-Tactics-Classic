using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Data", menuName = "Tower Data")]
public class TowerData : ScriptableObject
{
    public string description;
    public List<TraitData> traits;
    public Sprite sprite;
    public Sprite destroyedSprite;

    [System.Serializable]
    public class LevelStats
    {
        public int health;
        public float hitSpeed;
        public int damage;
        public float laneRange = 30.0f;
        public float areaRange;
    }
    public List<LevelStats> stats = new List<LevelStats>();

    public GameObject bulletObject;
    public float bulletSpeed;
}
