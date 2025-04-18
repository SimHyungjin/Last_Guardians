using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        target.ApplyReducionDef(towerData.EffectValue,towerData.EffectDuration);
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if(Random.value<chance)
            target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
    }

}
