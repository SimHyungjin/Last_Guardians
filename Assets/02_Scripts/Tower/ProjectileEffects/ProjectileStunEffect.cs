using UnityEngine;

public class ProjectileStunEffect : MonoBehaviour, IEffect
{
    ///////////=========================스턴 이펙트=================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.ApplySturn(adaptedTowerData.effectValue, adaptedTowerData.effectDuration);
            Debug.Log($"[Apply] {target.name} 스턴 적용");
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.ApplySturn(adaptedTowerData.effectDuration, adaptedTowerData.effectValue);
                Debug.Log($"[Apply] {target.name} 스턴 적용");
            }
        }
    }
}
