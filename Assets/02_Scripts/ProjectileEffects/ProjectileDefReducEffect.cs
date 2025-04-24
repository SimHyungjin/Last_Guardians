using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {

            target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
            Debug.Log($"±âº» ¹æ±ï {towerData.EffectValue}, {towerData.EffectDuration}");
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
                Debug.Log($"Âù½º ¹æ±ï {towerData.EffectValue}, {towerData.EffectDuration}");
            }
        }
    }
}
