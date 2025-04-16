using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasionBuffSkill : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        caster.ApplyEvasionBuff(skillData.Duration, skillData.MonsterskillEffectValue);
    }
}
