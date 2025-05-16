using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmSkilll : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        Collider2D[] collider2Ds = Utils.OverlapCircleAllSorted((Vector2)caster.transform.position, skillData.SkillRange, LayerMask.GetMask("Tower"));
        foreach (Collider2D var in collider2Ds)
        {
            if (var.gameObject.TryGetComponent<BaseTower>(out BaseTower baseTower))
            {
                //타워 무력화
                baseTower.OnDisabled();
            }
        }
    }
}
