using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossDebuffEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.ApplySkillValueDebuff(towerData.EffectValue, towerData.EffectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData,AdaptedTowerData adaptedTowerData ,float chance)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.ApplySkillValueDebuff(towerData.EffectValue, towerData.EffectDuration);
        }
    }
}
