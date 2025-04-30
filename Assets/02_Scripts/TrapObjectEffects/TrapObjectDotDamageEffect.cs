using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectDotDamageEffect : MonoBehaviour , ITrapEffect
{
    ///////////==========================도트데미지 이펙트================================/////////////////////

    public void Apply(BaseMonster target, TowerData towerData, bool bossImmunebuff, EnvironmentEffect environmentEffect) 
    {
        target.DotDamage(towerData.EffectValue, 0.1f);
    }
}
