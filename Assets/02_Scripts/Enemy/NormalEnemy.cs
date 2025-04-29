using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : BaseMonster
{
    public override void MeleeAttack()
    {
        base.MeleeAttack();
        if (!firstHit)
        {
            firstHit = true;
            InGameManager.Instance.TakeDmage(FirstHitDamage);
            Debug.Log($"노말공격 데미지 {FirstHitDamage}");
        }
        else
        {
            InGameManager.Instance.TakeDmage(SecondHitDamage);
            Debug.Log($"노말공격 데미지 {SecondHitDamage}");
        }
        AttackTimer = attackDelay;
        AfterAttack();
    }

    public override void RangeAttack()
    {
        base.RangeAttack();
        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.Data = MonsterData;
        projectile.BaseMonster = this;
        projectile.Launch(Target.transform.position);
        AttackTimer = attackDelay;
        AfterAttack();
    }

    public override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
