using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : BaseMonster
{
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        DamageText damageText = PoolManager.Instance.Spawn<DamageText>(MonsterManager.Instance.DamageTextPrefab);
        damageText.gameObject.transform.SetParent(MonsterManager.Instance.DamageUICanvas.transform);
        Vector3 worldPos = transform.position + Vector3.up * 0.1f;
        worldPos.z = 0;
        damageText.transform.position = worldPos;
        damageText.Show(amount);
    }

    protected override void MeleeAttack()
    {
        base.MeleeAttack();
        if (!firstHit)
        {
            firstHit = true;
            InGameManager.Instance.TakeDmage(5);
            Debug.Log($"보스몬스터 {monsterData.name} 공격 데미지 : 5");
        }
        else
        {
            InGameManager.Instance.TakeDmage(2);
            Debug.Log("보스몬스터 {monsterData.name} 공격 데미지 2");
        }
        
        attackTimer = attackDelay;
    }

    protected override void RangeAttack()
    {
        base.RangeAttack();
        if (!firstHit)
        {
            firstHit = true;
            InGameManager.Instance.TakeDmage(5);
            Debug.Log($"보스몬스터 {monsterData.name} 공격 데미지 : 5");
        }
        else
        {
            InGameManager.Instance.TakeDmage(2);
            Debug.Log("보스몬스터 {monsterData.name} 공격 데미지 2");
        }
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
