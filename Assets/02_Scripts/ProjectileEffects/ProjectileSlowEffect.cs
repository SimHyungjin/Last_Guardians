using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSlowEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        target.ApplySlowdown(towerData.EffectValue, towerData.EffectDuration);
    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (Random.value < chance)
        {
            target.ApplySlowdown(towerData.EffectValue, towerData.EffectDuration);
        }
    }
}
