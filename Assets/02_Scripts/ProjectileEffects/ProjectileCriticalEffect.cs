

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCriticalEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        target.TakeDamage(adaptedTowerData.attackPower * (towerData.EffectValue - 1));
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance,EnvironmentEffect environmentEffect)
    {
        if (Random.value < chance)
            target.TakeDamage(adaptedTowerData.attackPower*(towerData.EffectValue-1));
    }
}