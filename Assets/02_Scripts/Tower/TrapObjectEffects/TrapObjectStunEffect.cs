using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectStunEffect : MonoBehaviour, ITrapEffect
{
    ///////////==========================Ω∫≈œ ¿Ã∆Â∆Æ================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmunebuff))
        {
            target.ApplySturn(adaptedTrapObjectData.effectValue, adaptedTrapObjectData.effectDuration);
        }
    }
}

