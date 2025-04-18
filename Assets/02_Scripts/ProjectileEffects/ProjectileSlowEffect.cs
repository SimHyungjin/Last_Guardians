using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSlowEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if (!towerData.BossImmune)
        {
            target.ApplySlowdown(towerData.EffectValue, towerData.EffectDuration);
            Debug.Log($"기본 슬로우 {towerData.EffectValue},{towerData.EffectDuration}");
        }
        else if (target.MonsterData.MonsterType != MonType.Boss)
        {
            target.ApplySlowdown(towerData.EffectValue, towerData.EffectDuration);
            Debug.Log($"기본 슬로우 {towerData.EffectValue},{towerData.EffectDuration}");
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (!towerData.BossImmune)
        {
            if (Random.value < chance)
            {
                target.ApplySlowdown(towerData.EffectValue, towerData.EffectDuration);
                Debug.Log($"찬스 슬로우 {towerData.EffectValue},{towerData.EffectDuration}");
            }
        }
        else if (target.MonsterData.MonsterType != MonType.Boss)
        {
            if (Random.value < chance)
            {
                target.ApplySlowdown(towerData.EffectValue, towerData.EffectDuration);
                Debug.Log($"찬스 슬로우 {towerData.EffectValue},{towerData.EffectDuration}");
            }
        }
    }
}
