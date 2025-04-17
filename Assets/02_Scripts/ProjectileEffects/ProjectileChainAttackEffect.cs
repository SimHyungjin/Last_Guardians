using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileChainAttackEffect : MonoBehaviour
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if(towerData.EffectTarget==EffectTarget.All)
        {
            target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
        }
        else
        {
            BossMonster bossMonster = target.GetComponent<BossMonster>();
            if(bossMonster != null)
            {
                bossMonster.TakeDamage(towerData.AttackPower * towerData.EffectValue);
            }
        }
        
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        throw new System.NotImplementedException();
    }
}
