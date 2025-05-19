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
            target.ApplyKnockBack(0.3f, 0.5f, this.transform.position);
        }
    }
}
