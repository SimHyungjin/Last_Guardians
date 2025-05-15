using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStunEffect : MonoBehaviour,IEffect
{
    ///////////=========================���� ����Ʈ=================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData,EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.ApplySturn(towerData.EffectValue, towerData.EffectDuration);
            Debug.Log($"[Apply] {target.name} ���� ����");
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.ApplySturn(towerData.EffectDuration, towerData.EffectValue);
                Debug.Log($"[Apply] {target.name} ���� ����");
            }
        }
    }
}
