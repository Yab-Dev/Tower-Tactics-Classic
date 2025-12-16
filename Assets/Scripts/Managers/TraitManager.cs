using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private TraitData gathererTrait;



    private void Awake()
    {
        GameManager.OnDefensePhaseStart += DefenseStart;
    }

    private void OnDisable()
    {
        GameManager.OnDefensePhaseStart -= DefenseStart;
    }

    private void DefenseStart(int waveCount)
    {
        List<(TraitData trait, int count)> traitData = GameManager.GetInstance().GetCurrentTraits();
        (TraitData trait, int count) trait = GetTrait(traitData, gathererTrait);
        if (trait.trait != null)
        {
            if (trait.count >= trait.trait.breakpoints[0].breakpointValue)
            {
                Debug.Log("GATHERER BREAKPOINT REACHED");
            }
        }
    }

    private (TraitData trait, int count) GetTrait(List<(TraitData trait, int count)> traitData, TraitData trait)
    {
        for (int i = 0; i < traitData.Count; i++)
        {
            if (traitData[i].trait == trait)
            {
                return traitData[i];
            }
        }

        return (null, 0);
    }
}
