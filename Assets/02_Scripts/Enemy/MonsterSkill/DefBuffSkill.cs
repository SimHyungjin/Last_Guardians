using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBuffSkill : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        caster.ApplyDefBuff(skillData.Duration, skillData.MonsterskillEffectValue);
    }
}
