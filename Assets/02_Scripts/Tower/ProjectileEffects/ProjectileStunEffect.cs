using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStunEffect : MonoBehaviour,IEffect
{
    ///////////=========================Ω∫≈œ ¿Ã∆Â∆Æ=================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData,EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.ApplySturn(towerData.EffectValue, towerData.EffectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.ApplySturn(towerData.EffectValue, towerData.EffectDuration);
            }
        }
    }
}
