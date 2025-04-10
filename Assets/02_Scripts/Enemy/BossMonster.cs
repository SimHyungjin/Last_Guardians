using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : BaseMonster
{
    protected override void Attack()
    {
        base.Attack();
        Debug.Log($"보스몬스터 {monsterData.name} 공격");
        attackTimer = attackDelay;
    }

    protected override void MonsterSkill()
    {
        Debug.Log($"{monsterData.name} {skillData.name} 사용");
        skillTimer = skillData.SkillCoolTime;
    }

    protected override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
