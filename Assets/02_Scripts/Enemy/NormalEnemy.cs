using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : BaseMonster
{
    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        Debug.Log("노말공격");
        attackTimer = attackDelay;
    }

    protected override void RangeAttack()
    {
        base.RangeAttack();
        Debug.Log("노말원거리공격");
        attackTimer = attackDelay;

        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.Data = monsterData;
        projectile.Launch(Target.transform.position);
    }

    protected override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
