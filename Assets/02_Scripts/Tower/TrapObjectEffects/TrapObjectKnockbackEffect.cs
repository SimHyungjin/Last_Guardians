using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectKnockbackEffect : MonoBehaviour,ITrapEffect
{
    ///////////==========================�˹� ����Ʈ================================/////////////////////

    public void Apply(BaseMonster target,TowerData towerData, AdaptedTrapObjectData adaptedTowerData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData,bossImmunebuff))
        {
            target.ApplyKnockBack(0.5f, 0.5f, this.transform.position);
        }
    }
}
