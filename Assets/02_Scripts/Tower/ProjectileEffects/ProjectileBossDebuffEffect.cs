using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossDebuffEffect : MonoBehaviour, IEffect
{
    ///////////============================���°���==============================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.ApplySkillValueDebuff(adaptedTowerData.effectValue, adaptedTowerData.effectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData,AdaptedAttackTowerData adaptedTowerData ,float chance,EnvironmentEffect environmentEffect)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            if (Random.value < chance)
                target.ApplySkillValueDebuff(adaptedTowerData.effectValue, adaptedTowerData.effectDuration);
        }
    }
}
