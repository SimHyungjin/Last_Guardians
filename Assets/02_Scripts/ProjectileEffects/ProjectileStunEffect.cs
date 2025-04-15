using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStunEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        target.ApplySturn(towerData.EffectValue, towerData.EffectDuration);
    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (Random.value < chance)
        {
            target.ApplySturn(towerData.EffectValue, towerData.EffectDuration);
        }
    }
}
