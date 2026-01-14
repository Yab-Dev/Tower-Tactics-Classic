using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletBehavior;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TraitData gathererTrait;
    [SerializeField] private TraitData medievalTrait;
    [SerializeField] private TraitData explosiveTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject gathererFallingTokensPrefab;
    [SerializeField] private GameObject gathererStaticTokenPrefab;
    [SerializeField] private GameObject medievalDestroyedArrowInteractPrefab;
    [SerializeField] private GameObject medievalArrowTargetSpawnerPrefab;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed += GathererBreakpoint2;

        TowerBehavior.OnAnyTowerDestroyed += MedievalBreakpoint1;
        TowerBehavior.OnAnyTowerDestroyed += MedievalBreakpoint2;
        GameManager.OnDefensePhaseStart += MedievalBreakpoint3;

        BulletBehavior.OnApplyBulletTags += ExplosionBreakpoint1;
        BulletBehavior.OnApplyBulletTags += ExplosionBreakpoint2;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed -= GathererBreakpoint2;

        TowerBehavior.OnAnyTowerDestroyed -= MedievalBreakpoint1;
        TowerBehavior.OnAnyTowerDestroyed -= MedievalBreakpoint2;
        GameManager.OnDefensePhaseStart -= MedievalBreakpoint3;

        BulletBehavior.OnApplyBulletTags -= ExplosionBreakpoint1;
        BulletBehavior.OnApplyBulletTags -= ExplosionBreakpoint2;
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

    private void MedievalBreakpoint2(TowerData _towerData, Vector2 _destroyPosition)
    {
        if (!TraitUtils.CheckTraitBreakpoint(medievalTrait, 1)) { return; }

        List<TowerBehavior> medievalTowers = GameManager.GetInstance().GetTowersOfTrait(medievalTrait);
        foreach (TowerBehavior tower in medievalTowers)
        {
            tower.AddStatModification(_hitSpeed: -0.05f);
        }
    }

    private void MedievalBreakpoint3(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(medievalTrait, 2)) { return; }

        Instantiate(medievalArrowTargetSpawnerPrefab);
    }

    private void ExplosionBreakpoint1(ref List<BulletTags> _tags, TowerData _towerData)
    {
        if (!_towerData.traits.Contains(explosiveTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(explosiveTrait, 0)) { return; }

        _tags.Add(BulletTags.Explosive1);
    }

    private void ExplosionBreakpoint2(ref List<BulletTags> _tags, TowerData _towerData)
    {
        if (!_towerData.traits.Contains(explosiveTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(explosiveTrait, 0)) { return; }

        _tags.Add(BulletTags.Explosive2);
    }
}
