using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectDotDamageEffect : MonoBehaviour , ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData, bool bossImmunebuff) 
    {
        target.DotDamage(towerData.EffectValue, 0.1f);
    }
}
