using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPylon : MonoBehaviour
{
    private void Awake()
    {
        GameManager.OnDefensePhaseEnd += SelfDestruct;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseEnd -= SelfDestruct;
    }

    private void SelfDestruct(int _waveCount)
    {
        Destroy(gameObject);
    }
}
