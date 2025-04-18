using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if (!towerData.BossImmune)
            target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
        else if (target.MonsterData.MonsterType != MonType.Boss)
            target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (!towerData.BossImmune)
        {
            if (Random.value < chance)
                target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
        }
        else if (target.MonsterData.MonsterType != MonType.Boss)
        {
            if (Random.value < chance)
                target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
        }
    }
}
