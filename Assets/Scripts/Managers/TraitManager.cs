using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TraitData gathererTrait;

    [Header("Prefabs")]
    [SerializeField] private GameObject gathererFallingTokensPrefab;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += DefenseStart;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= DefenseStart;
    }

    private void DefenseStart(int _waveCount)
    {
        List<(TraitData trait, int count)> traitData = GameManager.GetInstance().GetCurrentTraits();
        (TraitData trait, int count) gathererTraitData = GetTrait(traitData, gathererTrait);
        if (gathererTraitData.trait != null)
        {
            if (gathererTraitData.count >= gathererTraitData.trait.breakpoints[0].breakpointValue)
            {
                Instantiate(gathererFallingTokensPrefab);
            }
        }
    }

    private (TraitData trait, int count) GetTrait(List<(TraitData trait, int count)> _traitData, TraitData _trait)
    {
        for (int i = 0; i < _traitData.Count; i++)
        {
            if (_traitData[i].trait == _trait)
            {
                return _traitData[i];
            }
        }

        return (null, 0);
    }
}
