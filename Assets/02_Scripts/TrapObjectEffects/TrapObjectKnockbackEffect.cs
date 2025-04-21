using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectKnockbackEffect : MonoBehaviour,ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData,bool bossImmunebuff)
    {
        if (Utils.ShouldApplyEffect(target, towerData,bossImmunebuff))
        {
            target.ApplyKnockBack(0.5f, 1f, this.transform.position);
        }
    }
}
