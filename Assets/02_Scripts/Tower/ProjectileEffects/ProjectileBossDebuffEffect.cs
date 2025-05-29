using UnityEngine;

public class ProjectileBossDebuffEffect : MonoBehaviour, IEffect
{
    ///////////============================방어력감소==============================/////////////////////
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            target.ApplySkillValueDebuff(adaptedTowerData.effectValue, adaptedTowerData.effectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffectm, bool bossImmune)
    {
        if (target.MonsterData.MonsterType == MonType.Boss)
        {
            if (Random.value < chance)
                target.ApplySkillValueDebuff(adaptedTowerData.effectValue, adaptedTowerData.effectDuration);
        }
    }
}
