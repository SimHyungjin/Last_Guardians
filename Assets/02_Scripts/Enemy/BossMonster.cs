using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : BaseMonster
{
    public override void TakeDamage(float amount, float penetration = 0, bool trueDamage = false)
    {
        base.TakeDamage(amount, penetration, trueDamage);
        //공격받을떄 데미지 텍스트 띄우기
        DamageText damageText = PoolManager.Instance.Spawn<DamageText>(InGameManager.Instance.DamageTextPrefab);
        damageText.gameObject.transform.SetParent(InGameManager.Instance.DamageUICanvas.transform);
        Vector3 worldPos = transform.position + Vector3.up * 0.1f;
        worldPos.z = 0;
        damageText.transform.position = worldPos;
        if (trueDamage)
            damageText.Show(amount);
        else
            damageText.Show(amount * (1 - CurrentDef * (1 - penetration) / (CurrentDef * (1 - penetration) + DefConstant)));
    }

    public override void MeleeAttack()
    {
        //근접공격시
        base.MeleeAttack();
        if (!firstHit)
        {
            firstHit = true;
            InGameManager.Instance.TakeDmage(FirstHitDamage);
        }
        else
        {
            InGameManager.Instance.TakeDmage(SecondHitDamage);
        }
        AttackTimer = attackDelay;
        AfterAttack();
    }

    public override void RangeAttack()
    {
        base.RangeAttack();
        EnemyProjectile projectile = PoolManager.Instance.Spawn<EnemyProjectile>(MonsterManager.Instance.ProjectilePrefab, this.transform);
        projectile.transform.SetParent(PoolManager.Instance.transform);
        projectile.Data = MonsterData;
        projectile.BaseMonster = this;
        projectile.Launch(Target.transform.position);
        AttackTimer = attackDelay;
        AfterAttack();
    }

    protected override void MonsterSkill()
    {
        //스킬사용
        Debug.Log($"{MonsterData.name} {MonsterSkillBaseData.skillData.name} 사용");
        MonsterSkillBaseData.UseSkill(this);
        SkillTimer = MonsterSkillBaseData.skillData.SkillCoolTime;
    }

    public override void Death()
    {
        base.Death();
        MonsterManager.Instance.BossKillCount++;
        PoolManager.Instance.Despawn(this);
    }
}
