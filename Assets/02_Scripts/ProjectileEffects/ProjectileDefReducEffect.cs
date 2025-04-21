using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
            Debug.Log($"�⺻ ���ο� {towerData.EffectValue}, {towerData.EffectDuration}");
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.ApplyReducionDef(towerData.EffectValue, towerData.EffectDuration);
                Debug.Log($"���� ���ο� {towerData.EffectValue}, {towerData.EffectDuration}");
            }
        }
    }
}
