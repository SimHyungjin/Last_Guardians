using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDotDamageEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        if (Random.value < chance)
        {
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration);
        }
    }
}
