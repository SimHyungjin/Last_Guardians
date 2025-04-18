using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSilenceEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.ApplySilenceDebuff(towerData.EffectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {

    }
}
