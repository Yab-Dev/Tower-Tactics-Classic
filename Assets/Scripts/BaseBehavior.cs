using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour, IDamage
{
    public delegate void OnBaseAttackedEventArgs();
    public static event OnBaseAttackedEventArgs OnBaseAttacked;

    public void Damage(int _amount)
    {
        OnBaseAttacked?.Invoke();
    }

    public IDamage.Team GetTeam()
    {
        return IDamage.Team.Tower;
    }
}
