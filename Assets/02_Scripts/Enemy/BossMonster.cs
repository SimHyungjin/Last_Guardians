using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : BaseMonster
{
    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        Debug.Log($"보스몬스터 {monsterData.name} 공격");
        attackTimer = attackDelay;
    }

    protected override void RangeAttack()
    {
        base.RangeAttack();
        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.Data = monsterData;
        projectile.Launch(Target.transform.position);
    }

    protected override void MonsterSkill()
    {
        Debug.Log($"{monsterData.name} {monsterSkill.skillData.name} 사용");
        monsterSkill.UseSkill(this);
        skillTimer = monsterSkill.skillData.SkillCoolTime;
    }

    protected override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
