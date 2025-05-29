using UnityEngine;

public class ProjectileStunEffect : MonoBehaviour, IEffect
{
    ///////////=========================스턴 이펙트=================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmune))
        {
            target.ApplySturn(adaptedTowerData.effectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        if (target == null)
        {
            return; 
        }
        if (Utils.ShouldApplyEffect(target, towerData, bossImmune))
        {
            if (Random.value < chance)
            {
                target.ApplySturn(adaptedTowerData.effectDuration);
            }
        }
    }
}
