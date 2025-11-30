using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string description;
    public Sprite sprite;

    public int health;
    public float hitSpeed;
    public int damage;
    public float range;
    public float moveSpeed;
}
