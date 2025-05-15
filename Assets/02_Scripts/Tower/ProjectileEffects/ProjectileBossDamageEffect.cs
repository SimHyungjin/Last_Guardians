using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossDamageEffect : MonoBehaviour, IEffect
{
    ///////////=====================보스추가데미지=====================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.TakeDamage(adaptedTowerData.attackPower * adaptedTowerData.effectValue);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance,EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            if (Random.value < chance)
                target.TakeDamage(adaptedTowerData.attackPower * adaptedTowerData.effectValue);
        }
    }
}
