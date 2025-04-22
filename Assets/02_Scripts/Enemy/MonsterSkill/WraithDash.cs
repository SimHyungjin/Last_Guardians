using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithDash : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        caster.ApplySpeedBuff(caster.CurrentSkillValue, skillData.Duration);
    }
}
