using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TraitData gathererTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject gathererFallingTokensPrefab;
    [SerializeField] private GameObject gathererStaticTokenPrefab;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed += GathererBreakpoint2;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed -= GathererBreakpoint2;
    }



    private void GathererBreakpoint1(int _waveCount)
    {
        if (TraitUtils.CheckTraitBreakpoint(gathererTrait, 0))
        {
            Instantiate(gathererFallingTokensPrefab);
        }
    }

    private void GathererBreakpoint2(Vector2 _deathPosition)
    {
        if (TraitUtils.CheckTraitBreakpoint(gathererTrait, 1))
        {
            Instantiate(gathererStaticTokenPrefab, _deathPosition, Quaternion.identity);
        }
    }
}
