using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossDamageEffect : MonoBehaviour, IEffect
{
    ///////////=====================보스추가데미지=====================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance,EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            if (Random.value < chance)
                target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
        }
    }
}
