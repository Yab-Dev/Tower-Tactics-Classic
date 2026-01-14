using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamage
{
    public enum Team
    {
        Tower,
        Enemy,
    }
    public Team GetTeam();
    public void Damage(int _amount);
    public void ApplyTags(List<BulletBehavior.BulletTags> _tags);
}