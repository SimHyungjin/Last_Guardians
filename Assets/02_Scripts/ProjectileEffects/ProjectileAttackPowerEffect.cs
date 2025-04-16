using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackPowerEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        throw new System.NotImplementedException();
    }
}
