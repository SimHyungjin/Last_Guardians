using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyMonster : BaseMonster
{
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        DamageText damageText = PoolManager.Instance.Spawn<DamageText>(InGameManager.Instance.DamageTextPrefab);
        damageText.gameObject.transform.SetParent(InGameManager.Instance.DamageUICanvas.transform);
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
            InGameManager.Instance.TakeDmage(FirstHitDamage);
            Debug.Log($"현상금몬스터 {MonsterData.name} 공격 데미지 : {FirstHitDamage}");
        }
        else
        {
            InGameManager.Instance.TakeDmage(SecondHitDamage);
            Debug.Log($"현상금몬스터 {MonsterData.name} 공격 데미지 {SecondHitDamage}");
        }

        attackTimer = attackDelay;
        AfterAttack();
    }

    protected override void MonsterSkill()
    {
        Debug.Log($"{MonsterData.name} {MonsterSkillBaseData.skillData.name} 사용");
        MonsterSkillBaseData.UseSkill(this);
        skillTimer = MonsterSkillBaseData.skillData.SkillCoolTime;
    }

    protected override void RangeAttack()
    {
        base.RangeAttack();
        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.Data = MonsterData;
        projectile.BaseMonster = this;
        projectile.Launch(Target.transform.position);
        attackTimer = attackDelay;
        AfterAttack();
    }
    protected override void Death()
    {
        base.Death();
        PoolManager.Instance.Despawn(this);
    }
}
