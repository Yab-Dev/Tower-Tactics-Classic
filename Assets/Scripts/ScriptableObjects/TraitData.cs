using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trait Data", menuName = "Trait Data")]
public class TraitData : ScriptableObject
{
    public string description;

    public Sprite traitIcon;

    public int[] breakpoints;
}
