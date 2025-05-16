using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaHealing : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        Collider2D[] collider2Ds = Utils.OverlapCircleAllSorted((Vector2)caster.transform.position, skillData.SkillRange, LayerMask.GetMask("Monster"));
        foreach (Collider2D var in collider2Ds)
        {
            if (var.gameObject.TryGetComponent<BaseMonster>(out BaseMonster baseMonster))
            {
                baseMonster.CurrentHP = Mathf.Max(baseMonster.CurrentHP * baseMonster.CurrentSkillValue + baseMonster.CurrentHP, baseMonster.ResultHP);
                //baseMonster.CurrentHP += baseMonster.CurrentHP * baseMonster.CurrentSkillValue;
            }
        }
    }
}
