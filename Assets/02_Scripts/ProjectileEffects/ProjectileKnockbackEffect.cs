using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileKnockbackEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        target.ApplyKnockBack(towerData.EffectDuration, 1f, this.transform.position);
    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
        target.ApplyKnockBack(towerData.EffectDuration, 1f, this.transform.position);
    }
}
