using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyMonster : BaseMonster
{
    protected override void Attack()
    {
        base.Attack();
        Debug.Log("현상금몬스터공격");
        attackTimer = attackDelay;
    }

    protected override void MonsterSkill()
    {
        Debug.Log($"현상금몬스터 {skillData.name} 사용");
        skillTimer = skillData.skillCoolTime;
    }
}
