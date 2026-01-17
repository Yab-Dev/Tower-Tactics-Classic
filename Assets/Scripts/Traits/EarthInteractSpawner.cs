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
