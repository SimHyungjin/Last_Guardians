using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectSlowEffect : MonoBehaviour,ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData, bool bossImmunebuff)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmunebuff))
        {
            target.ApplySlowdown(towerData.EffectValue, 0.1f);
        }
    }
}

