using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectDotDamageEffect : MonoBehaviour , ITrapEffect
{
    ///////////==========================��Ʈ������ ����Ʈ================================/////////////////////

    public void Apply(BaseMonster target, TowerData towerData, bool bossImmunebuff, EnvironmentEffect environmentEffect) 
    {
        target.DotDamage(towerData.EffectValue, 0.1f);
    }
}
