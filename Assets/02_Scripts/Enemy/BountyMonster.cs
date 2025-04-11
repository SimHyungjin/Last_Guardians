using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyMonster : BaseMonster
{
    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        Debug.Log("현상금몬스터공격");
        attackTimer = attackDelay;
    }

    protected override void MonsterSkill()
    {
        Debug.Log($"현상금몬스터 {skillData.name} 사용");
        skillData.UseSkill(this);
        projectile.Data = monsterData;
        skillTimer = skillData.SkillCoolTime;
    }

    protected override void RangeAttack()
    {
        base.RangeAttack();
        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.Lauch(Target);
    }
    protected override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
