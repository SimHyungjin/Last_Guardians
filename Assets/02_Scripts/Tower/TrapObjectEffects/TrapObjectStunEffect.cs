using UnityEngine;

public class TrapObjectStunEffect : MonoBehaviour, ITrapEffect
{
    ///////////==========================스턴 이펙트================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmunebuff))
        {
            target.ApplySturn(adaptedTrapObjectData.effectValue, adaptedTrapObjectData.effectDuration);
        }
    }
}

