using UnityEngine;

public class BountyMonster : BaseMonster
{
    public override void TakeDamage(float amount, float penetration = 0, bool trueDamage = false)
    {
        base.TakeDamage(amount, penetration, trueDamage);
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

    protected override void MonsterSkill()
    {
        Debug.Log($"{MonsterData.name} {MonsterSkillBaseData.skillData.name} 사용");
        MonsterSkillBaseData.UseSkill(this);
        SkillTimer = MonsterSkillBaseData.skillData.SkillCoolTime;
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
