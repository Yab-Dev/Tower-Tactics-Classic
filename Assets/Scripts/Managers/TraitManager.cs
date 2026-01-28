using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float medievalBreakpoint2AttackSpeedBoost = -0.05f;
    [SerializeField] private int wallBreakpoint1HealthPerLevel = 6;
    [SerializeField] private TowerData icyBreakpointTowerData;
    [SerializeField] private float crystallizedExplosionRadius;
    [SerializeField] private int crystallizedExplosionDamage;

    [Header("Cache")]
    [SerializeField] private TraitData gathererTrait;
    [SerializeField] private TraitData medievalTrait;
    [SerializeField] private TraitData explosiveTrait;
    [SerializeField] private TraitData sniperTrait;
    [SerializeField] private TraitData wallTrait;
    [SerializeField] private TraitData fieryTrait;
    [SerializeField] private TraitData icyTrait;
    [SerializeField] private TraitData earthyTrait;
    [SerializeField] private TraitData throneTrait;
    [SerializeField] private TraitData steamTrait;
    [SerializeField] private TraitData gamblerTrait;
    [SerializeField] private TraitData ancientTrait;
    [SerializeField] private TraitData crystallizedTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject gathererFallingTokensPrefab;
    [SerializeField] private GameObject gathererStaticTokenPrefab;
    [SerializeField] private GameObject medievalDestroyedArrowInteractPrefab;
    [SerializeField] private GameObject medievalArrowTargetSpawnerPrefab;
    [SerializeField] private GameObject wallHealerPrefab;
    [SerializeField] private GameObject igniteWallPrefab;
    [SerializeField] private GameObject fieryDamageAllPrefab;
    [SerializeField] private GameObject earthyKnockbackBulletSpawner;
    [SerializeField] private GameObject earthyLaneSwitchSpawner;
    [SerializeField] private GameObject throneBuffPrefab;
    [SerializeField] private GameObject steamValvePrefab;
    [SerializeField] private GameObject tokenCasinoPrefab;
    [SerializeField] private GameObject pterodactylPrefab;
    [SerializeField] private GameObject crystalExplosionPrefab;
    [SerializeField] private GameObject crystalPylonPrefab;
    [SerializeField] private GameObject crystalLaserSpawnerPrefab;

    private GameObject igniteWallObject;
    private GameObject fieryDamageAllObject;
    private GameObject icePillar1;
    private GameObject icePillar2;
    private GameObject icePillar3;
    private GameObject pterodactylObject;



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

        GameManager.OnApplyStaticEffects += FieryBreakpoint1;
        GameManager.OnApplyStaticEffects += FieryBreakpoint2;

        BulletBehavior.OnApplyBulletTags += IcyBreakpoint1;
        GameManager.OnApplyStaticEffects += IcyBreakpoint2;
        GameManager.OnApplyStaticEffects += IcyBreakpoint3;

        GameManager.OnDefensePhaseStart += EarthyBreakpoint1;
        GameManager.OnDefensePhaseStart += EarthyBreakpoint2;

        GameManager.OnDefensePhaseStart += ThroneBeakpoint1;
        GameManager.OnDefensePhaseStart += SteamBreakpoint1;
        GameManager.OnDefensePhaseStart += GamblerBreakpoint1;
        TowerBehavior.OnAnyTowerDestroyed += AncientBreakpoint1;

        TowerBehavior.OnAnyTowerDestroyed += CrystallizedBeakpoint2;
        BulletBehavior.OnApplyBulletTags += CrystallizedBeakpoint3;
        GameManager.OnDefensePhaseStart += CrystallizedBeakpoint4;
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

        GameManager.OnApplyStaticEffects -= FieryBreakpoint1;
        GameManager.OnApplyStaticEffects -= FieryBreakpoint2;

        BulletBehavior.OnApplyBulletTags -= IcyBreakpoint1;
        GameManager.OnApplyStaticEffects -= IcyBreakpoint2;
        GameManager.OnApplyStaticEffects -= IcyBreakpoint3;

        GameManager.OnDefensePhaseStart -= EarthyBreakpoint1;
        GameManager.OnDefensePhaseStart -= EarthyBreakpoint2;

        GameManager.OnDefensePhaseStart -= ThroneBeakpoint1;
        GameManager.OnDefensePhaseStart -= SteamBreakpoint1;
        GameManager.OnDefensePhaseStart -= GamblerBreakpoint1;

        TowerBehavior.OnAnyTowerDestroyed -= CrystallizedBeakpoint2;
        BulletBehavior.OnApplyBulletTags -= CrystallizedBeakpoint3;
        GameManager.OnDefensePhaseStart += CrystallizedBeakpoint4;
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

    private void FieryBreakpoint1()
    {
        if (TraitUtils.CheckTraitBreakpoint(fieryTrait, 0))
        {
            if (igniteWallObject != null) { return; }
            igniteWallObject = Instantiate(igniteWallPrefab);
        }
        else if (igniteWallObject != null)
        {
            Destroy(igniteWallObject);
        }
    }

    private void FieryBreakpoint2()
    {
        if (TraitUtils.CheckTraitBreakpoint(fieryTrait, 1))
        {
            if (fieryDamageAllObject != null) { return; }
            fieryDamageAllObject = Instantiate(fieryDamageAllPrefab);
        }
        else if (fieryDamageAllObject != null)
        {
            Destroy(fieryDamageAllObject);
        }
    }

    private void IcyBreakpoint1(ref List<BulletBehavior.BulletTags> _tags, TowerData _towerData)
    {
        if (_towerData == null) { return; }
        if (!_towerData.traits.Contains(icyTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(icyTrait, 0)) { return; }

        _tags.Add(BulletBehavior.BulletTags.Icy);
    }

    private void IcyBreakpoint2()
    {
        if (TraitUtils.CheckTraitBreakpoint(icyTrait, 1))
        { 
            if (icePillar1 != null) { return; }
            icePillar1 = GameManager.GetInstance().SpawnTowerOnSlot(icyBreakpointTowerData);
        }
        else if (icePillar1 != null)
        {
            Destroy(icePillar1);
            icePillar1 = null;
            GameManager.GetInstance().UpdateCurrentTowers();
        }
    }

    private void IcyBreakpoint3()
    {
        if (TraitUtils.CheckTraitBreakpoint(icyTrait, 2))
        {
            if (icePillar2 == null)
            {
                icePillar2 = GameManager.GetInstance().SpawnTowerOnSlot(icyBreakpointTowerData);
            }
            if (icePillar3 == null)
            {
                icePillar3 = GameManager.GetInstance().SpawnTowerOnSlot(icyBreakpointTowerData);
            }
        }
        else
        {
            if (icePillar2 != null)
            {
                Destroy(icePillar2);
                icePillar2 = null;
                GameManager.GetInstance().UpdateCurrentTowers();
            }
            if (icePillar3 != null)
            {
                Destroy(icePillar3);
                icePillar3 = null;
                GameManager.GetInstance().UpdateCurrentTowers();
            }
        }
    }

    private void EarthyBreakpoint1(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(earthyTrait, 0)) { return; }

        Instantiate(earthyKnockbackBulletSpawner);
    }

    private void EarthyBreakpoint2(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(earthyTrait, 1)) { return; }

        Instantiate(earthyLaneSwitchSpawner);
    }

    private void ThroneBeakpoint1(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(throneTrait, 0)) { return; }

        Instantiate(throneBuffPrefab);
    }

    private void SteamBreakpoint1(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(steamTrait, 0)) { return; }

        Instantiate(steamValvePrefab, GameObject.FindWithTag("TraitCanvas").transform);
    }

    private void GamblerBreakpoint1(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(gamblerTrait, 0)) { return; }

        Instantiate(tokenCasinoPrefab, GameObject.FindWithTag("TraitCanvas").transform);
    }

    private void AncientBreakpoint1(TowerData _towerData, Vector2 _destroyPosition)
    {
        if (!TraitUtils.CheckTraitBreakpoint(ancientTrait, 0)) { return; }
        if (pterodactylObject != null) { return; }

        pterodactylObject = Instantiate(pterodactylPrefab, _destroyPosition, Quaternion.identity);
    }

    private void CrystallizedBeakpoint2(TowerData _towerData, Vector2 _destroyPosition)
    {
        if (!_towerData.traits.Contains(earthyTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(crystallizedTrait, 1)) { return; }

        Instantiate(crystalPylonPrefab, _destroyPosition, Quaternion.identity);
        ExplosionBehavior.CreateExplosion(crystalExplosionPrefab, _destroyPosition, IDamage.Team.Tower, crystallizedExplosionDamage, crystallizedExplosionRadius, new List<BulletBehavior.BulletTags>(), new BulletBehavior.IgniteData(), 0, 0);
    }

    private void CrystallizedBeakpoint3(ref List<BulletBehavior.BulletTags> _tags, TowerData _towerData)
    {
        if (_towerData == null) { return; }
        if (!_towerData.traits.Contains(earthyTrait)) { return; }
        if (!TraitUtils.CheckTraitBreakpoint(crystallizedTrait, 2)) { return; }

        _tags.Add(BulletBehavior.BulletTags.Crystalline);
    }

    private void CrystallizedBeakpoint4(int _waveCount)
    {
        if (!TraitUtils.CheckTraitBreakpoint(crystallizedTrait, 3)) { return; }

        Instantiate(crystalLaserSpawnerPrefab);
    }
}
