using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossDamageEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            if (Random.value < chance)
                target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
        }
    }
}
