using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TraitData gathererTrait;
    [SerializeField] private TraitData medievalTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject gathererFallingTokensPrefab;
    [SerializeField] private GameObject gathererStaticTokenPrefab;
    [SerializeField] private GameObject medievalDestroyedArrowInteractPrefab;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed += GathererBreakpoint2;

        TowerBehavior.OnAnyTowerDestroyed += MedievalBreakpoint1;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed -= GathererBreakpoint2;

        TowerBehavior.OnAnyTowerDestroyed -= MedievalBreakpoint1;
    }



    private void GathererBreakpoint1(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(gathererTrait, 0)) {  return; }
        
        Instantiate(gathererFallingTokensPrefab);
    }

    private void GathererBreakpoint2(Vector2 _deathPosition)
    {
        if (!TraitUtils.CheckTraitBreakpoint(gathererTrait, 1)) { return; }
        
        Instantiate(gathererStaticTokenPrefab, _deathPosition, Quaternion.identity);
    }

    private void MedievalBreakpoint1(TowerData _towerData, Vector2 _destroyPosition)
    {
        if (!_towerData.traits.Contains(medievalTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(medievalTrait, 0)) { return; }

        Instantiate(medievalDestroyedArrowInteractPrefab, _destroyPosition, Quaternion.identity);
    }
}
