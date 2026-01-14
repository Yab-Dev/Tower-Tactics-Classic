using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Data", menuName = "Tower Data")]
public class TowerData : ScriptableObject
{
    [TextArea]
    public string description;
    public List<TraitData> traits;
    public int cost;
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
        public int hitCount = 1;

        public static LevelStats operator + (LevelStats original, LevelStats modifier)
        {
            LevelStats ret = new LevelStats ();

            ret.health = original.health + modifier.health;
            ret.hitSpeed = original.hitSpeed * modifier.hitSpeed;
            ret.damage = original.damage + modifier.damage;
            ret.laneRange = original.laneRange + modifier.laneRange;
            ret.areaRange = original.areaRange + modifier.areaRange;
            ret.hitCount = original.hitCount + modifier.hitCount;

            return ret;
        }
    }
    public List<LevelStats> stats = new List<LevelStats>();

    public GameObject bulletObject;
    public float bulletSpeed;
}
