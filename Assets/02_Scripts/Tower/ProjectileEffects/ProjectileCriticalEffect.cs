using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCriticalEffect : MonoBehaviour, IEffect
{
    ///////////==========================크리티컬 추가데미지================================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        target.TakeDamage(adaptedTowerData.attackPower * (adaptedTowerData.effectValue - 1));
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance,EnvironmentEffect environmentEffect)
    {
        if (Random.value < chance)
            target.TakeDamage(adaptedTowerData.attackPower*(adaptedTowerData.effectValue -1));
    }
}