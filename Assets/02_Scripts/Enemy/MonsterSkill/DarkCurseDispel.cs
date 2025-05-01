using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCurseDispel : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        Collider2D[] collider2Ds = Utils.OverlapCircleAllSorted((Vector2)caster.transform.position, skillData.SkillRange, LayerMask.GetMask("Monster"));
        foreach (Collider2D var in collider2Ds)
        {
            if (var.gameObject.TryGetComponent<BaseMonster>(out BaseMonster baseMonster))
            {
                //디버프 취소
                baseMonster.CancelAllDebuff();
            }
        }
    }

   
}
