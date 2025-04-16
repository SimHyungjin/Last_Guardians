using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : BaseMonster
{
    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        if (!firstHit)
        {
            firstHit = true;
            InGameManager.Instance.TakeDmage(1);
            Debug.Log("노말공격 데미지 1");
        }
        else
        {
            InGameManager.Instance.TakeDmage(2);
            Debug.Log("노말공격 데미지 2");
        }
        
        attackTimer = attackDelay;
    }

    protected override void RangeAttack()
    {
        base.RangeAttack();
        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.Data = monsterData;
        projectile.Launch(Target.transform.position);
        attackTimer = attackDelay;
    }

    protected override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
