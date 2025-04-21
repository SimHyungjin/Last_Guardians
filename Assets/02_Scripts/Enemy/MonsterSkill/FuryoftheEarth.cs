using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryoftheEarth : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        caster.ApplyDefBuff(skillData.Duration, caster.CurrentSkillValue);
    }
}
