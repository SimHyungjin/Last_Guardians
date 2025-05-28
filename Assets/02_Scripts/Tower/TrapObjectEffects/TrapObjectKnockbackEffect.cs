using UnityEngine;

public class TrapObjectKnockbackEffect : MonoBehaviour, ITrapEffect
{
    ///////////==========================넉백 이펙트================================/////////////////////

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmunebuff))
        {

            target.ApplyKnockBack(adaptedTrapObjectData.effectValue, adaptedTrapObjectData.effectValue, this.transform.position);
        }
    }
}
