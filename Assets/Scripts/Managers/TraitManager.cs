using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float medievalBreakpoint2AttackSpeedBoost = -0.05f;
    [SerializeField] private int wallBreakpoint1HealthPerLevel = 6;

    [Header("Cache")]
    [SerializeField] private TraitData gathererTrait;
    [SerializeField] private TraitData medievalTrait;
    [SerializeField] private TraitData explosiveTrait;
    [SerializeField] private TraitData sniperTrait;
    [SerializeField] private TraitData wallTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject gathererFallingTokensPrefab;
    [SerializeField] private GameObject gathererStaticTokenPrefab;
    [SerializeField] private GameObject medievalDestroyedArrowInteractPrefab;
    [SerializeField] private GameObject medievalArrowTargetSpawnerPrefab;
    [SerializeField] private GameObject wallHealerPrefab;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += GathererBreakpoint1;
        EnemyBehavior.OnAnyEnemyDestroyed += GathererBreakpoint2;

        TowerBehavior.OnAnyTowerDestroyed += MedievalBreakpoint1;
        TowerBehavior.OnAnyTowerDestroyed += MedievalBreakpoint2;
        GameManager.OnDefensePhaseStart += MedievalBreakpoint3;

        BulletBehavior.OnApplyBulletTags += ExplosionBreakpoint1;
        BulletBehavior.OnApplyBulletTags += ExplosionBreakpoint2;

        BulletBehavior.OnApplyBulletTags += SniperBreakpoint1;
        GameManager.OnApplyStaticEffects += SniperBreakpoint2;

        GameManager.OnApplyStaticEffects += WallBreakpoint1;
        TowerBehavior.OnAnyTowerHit += WallBreakpoint2;
        GameManager.OnDefensePhaseStart += WallBreakpoint3;
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

        BulletBehavior.OnApplyBulletTags -= SniperBreakpoint1;
        GameManager.OnApplyStaticEffects -= SniperBreakpoint2;

        GameManager.OnApplyStaticEffects -= WallBreakpoint1;
        TowerBehavior.OnAnyTowerHit -= WallBreakpoint2;
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
            tower.AddStatModification(_hitSpeed: medievalBreakpoint2AttackSpeedBoost);
        }
    }

    private void MedievalBreakpoint3(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(medievalTrait, 2)) { return; }

        Instantiate(medievalArrowTargetSpawnerPrefab);
    }

    private void ExplosionBreakpoint1(ref List<BulletBehavior.BulletTags> _tags, TowerData _towerData)
    {
        if (_towerData == null) { return; }
        if (!_towerData.traits.Contains(explosiveTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(explosiveTrait, 0)) { return; }

        _tags.Add(BulletBehavior.BulletTags.Explosive1);
    }

    private void ExplosionBreakpoint2(ref List<BulletBehavior.BulletTags> _tags, TowerData _towerData)
    {
        if (_towerData == null) { return; }
        if (!_towerData.traits.Contains(explosiveTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(explosiveTrait, 1)) { return; }

        _tags.Add(BulletBehavior.BulletTags.Explosive2);
    }

    private void SniperBreakpoint1(ref List<BulletBehavior.BulletTags> _tags, TowerData _towerData)
    {
        if (_towerData == null) { return; }
        if (!_towerData.traits.Contains(sniperTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(sniperTrait, 0)) { return; }

        _tags.Add(BulletBehavior.BulletTags.Sniper);
    }

    private void SniperBreakpoint2()
    {
        if (!TraitUtils.CheckTraitBreakpoint(sniperTrait, 1)) { return; }

        List<TowerBehavior> towers = GameManager.GetInstance().GetTowersOfTrait(null);
        foreach (TowerBehavior tower in towers)
        {
            if (tower.GetComponent<TowerDragDrop>().IsInRearSlot)
            {
                tower.AddStatModification(_hitCount: tower.TowerData.stats[tower.Level - 1].hitCount);
            }
        }
    }

    private void WallBreakpoint1()
    {
        if (!TraitUtils.CheckTraitBreakpoint(wallTrait, 0)) { return; }

        List<TowerBehavior> wallTowers = GameManager.GetInstance().GetTowersOfTrait(wallTrait);
        foreach (TowerBehavior tower in wallTowers)
        {
            tower.AddStatModification(_health: wallBreakpoint1HealthPerLevel * tower.Level);
            tower.CurrentHealth = tower.CurrentStats.health;
        }
    }

    private void WallBreakpoint2(TowerBehavior _towerBehavior)
    {
        if (!TraitUtils.CheckTraitBreakpoint(wallTrait, 1)) { return; }

        BulletBehavior.CreateBullet(_towerBehavior.TowerData.bulletObject, _towerBehavior.transform.position, null, IDamage.Team.Tower, _towerBehavior.CurrentStats.damage / 2, 5.0f, _towerData: _towerBehavior.TowerData);
    }

    private void WallBreakpoint3(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(wallTrait, 2)) { return; }

        Instantiate(wallHealerPrefab);
    }
}
