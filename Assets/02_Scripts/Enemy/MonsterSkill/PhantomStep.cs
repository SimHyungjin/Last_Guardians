using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomStep : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        caster.ApplyEvasionBuff(skillData.Duration, caster.CurrentSkillValue);
    }
}
