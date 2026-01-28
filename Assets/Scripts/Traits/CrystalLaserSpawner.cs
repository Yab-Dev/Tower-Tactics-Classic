using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLaserSpawner : MonoBehaviour
{
    private readonly string crystalPylonTag = "CrystalPylon";

    [Header("Attributes")]
    [SerializeField] private float laserSpawnSpeed;

    [Header("Cache")]
    [SerializeField] private TraitData crystalTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject crystalLaserPrefab;

    private float spawnTimer;



    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += SelfDestruct;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= SelfDestruct;
    }

    private void Start()
    {
        spawnTimer = laserSpawnSpeed;
    }

    private void Update()
    {
        if (spawnTimer <= 0.0f)
        {
            SpawnLasers();
            spawnTimer = laserSpawnSpeed;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void SpawnLasers()
    {
        GameObject[] crystalPylons = GameObject.FindGameObjectsWithTag(crystalPylonTag);
        List<TowerBehavior> crysalNexuses = GameManager.GetInstance().GetTowersOfTrait(crystalTrait);

        List<GameObject> crystalTargets = new List<GameObject>();

        foreach (GameObject pylon in crystalPylons)
        {
            crystalTargets.Add(pylon);
        }
        foreach (TowerBehavior nexus in crysalNexuses)
        {
            crystalTargets.Add(nexus.gameObject);
        }

        int targetIndex = Random.Range(0, crystalTargets.Count);
        while (crystalTargets.Count > 1)
        {
            GameObject startCrystal = crystalTargets[targetIndex];
            crystalTargets.RemoveAt(targetIndex);

            targetIndex = Random.Range(0, crystalTargets.Count);
            GameObject endCrystal = crystalTargets[targetIndex];

            Vector3 laserPosition = Vector3.Lerp(startCrystal.transform.position, endCrystal.transform.position, 0.5f);
            float laserWidth = Vector3.Distance(startCrystal.transform.position, endCrystal.transform.position);


            GameObject laser = Instantiate(crystalLaserPrefab, laserPosition, Quaternion.identity);
            laser.transform.localScale = new Vector3(laserWidth, 1.0f, 1.0f);

            Vector3 direction = endCrystal.transform.position - startCrystal.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }
}
