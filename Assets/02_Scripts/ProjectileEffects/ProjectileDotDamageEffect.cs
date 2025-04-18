using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDotDamageEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if (!towerData.BossImmune)
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
        else if (target.MonsterData.MonsterType != MonType.Boss)
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration);

    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (!towerData.BossImmune)
        {
            if (Random.value < chance)
                target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
        }
        else if (target.MonsterData.MonsterType != MonType.Boss)
        {
            if (Random.value < chance)
                target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
        }
    }
}
