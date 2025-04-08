using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : BaseMonster
{

    protected override void Attack()
    {
        Debug.Log("보스공격");
        attackTimer = attackDelay;
    }

    protected override void MonsterSkill()
    {
        Debug.Log($"보스몬스터 {skillData.name} 사용");
        skillTimer = skillData.skillCoolTime;
    }

}
