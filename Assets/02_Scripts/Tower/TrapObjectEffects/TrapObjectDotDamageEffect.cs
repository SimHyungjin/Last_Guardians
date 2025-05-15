using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectDotDamageEffect : MonoBehaviour , ITrapEffect
{
    ///////////==========================도트데미지 이펙트================================/////////////////////

    public void Apply(BaseMonster target,TowerData towerData, AdaptedTrapObjectData adaptedTowerData, bool bossImmunebuff, EnvironmentEffect environmentEffect) 
    {
        target.DotDamage(adaptedTowerData.effectValue, 0.6f);
    }
}
