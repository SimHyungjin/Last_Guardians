using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCry : MonsterSkillBase
{
    private Collider2D[] collider2Ds;
    public override void UseSkill(BaseMonster caster)
    {
        // 범위 선택
        collider2Ds = Utils.OverlapCircleAllSorted(caster.transform.position, skillData.SkillRange / 2f, LayerMask.GetMask("Monster"));
        foreach(Collider2D collider in collider2Ds)
        {
            if(collider.TryGetComponent<BaseMonster>(out BaseMonster baseMonster))
            {
                //속도버프
                baseMonster.ApplySpeedBuff(baseMonster.CurrentSkillValue, skillData.Duration);
            }
        }
    }

    
}
