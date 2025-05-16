using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectKnockbackEffect : MonoBehaviour,ITrapEffect
{
    ///////////==========================≥ÀπÈ ¿Ã∆Â∆Æ================================/////////////////////

    public void Apply(BaseMonster target,TowerData towerData, AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData,bossImmunebuff))
        {
            Debug.Log($"[TrapObjectStunEffect] {target.name} Ω∫≈œ ¿˚øÎ {towerData.TowerIndex} {adaptedTrapObjectData.effectValue}");
            target.ApplyKnockBack(0.5f, 0.5f, this.transform.position);
        }
    }
}
