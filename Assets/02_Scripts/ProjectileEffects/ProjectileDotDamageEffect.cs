using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDotDamageEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData)
    {
        if(target.MonsterData.MonsterType==MonType.Boss&&towerData.EffectTarget==EffectTarget.BossOnly)
        {
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
        }
        else if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance)
    {
        if (target.MonsterData.MonsterType == MonType.Boss && towerData.EffectTarget == EffectTarget.BossOnly)
        {
            if (Random.value < chance)
            {
                target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
            }
        }
        else if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
            }
        }
    }
}
