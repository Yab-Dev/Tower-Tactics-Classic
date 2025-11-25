using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Data", menuName = "Tower Data")]
public class TowerData : ScriptableObject
{
    public string description;
    public TraitData[] traits;

    public int health;
    public float hitSpeed;
    public int damage;

    public GameObject bullet;
}
