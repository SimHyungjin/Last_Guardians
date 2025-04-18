using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossDebuffEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        target.ApplySkillValueDebuff(towerData.EffectValue,towerData.EffectDuration);
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        
    }
}
