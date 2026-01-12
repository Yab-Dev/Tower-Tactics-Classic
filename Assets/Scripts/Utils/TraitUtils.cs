using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitUtils : MonoBehaviour
{
    private static (TraitData trait, int count) GetTrait(List<(TraitData trait, int count)> _traitData, TraitData _trait)
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

    public static bool CheckTraitBreakpoint(TraitData _trait, int _breakpoint)
    {
        List<(TraitData trait, int count)> traitData = GameManager.GetInstance().GetCurrentTraits();
        (TraitData trait, int count) specificTraitData = GetTrait(traitData, _trait);
        if (specificTraitData.trait != null)
        {
            if (specificTraitData.count >= specificTraitData.trait.breakpoints[_breakpoint].breakpointValue)
            {
                return true;
            }
        }
        return false;
    }
}
