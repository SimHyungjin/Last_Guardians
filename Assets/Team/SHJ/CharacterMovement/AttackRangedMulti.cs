using UnityEngine;

public class AttackRangedMulti : IAttackBehavior
{
    private AttackController attackController;

    public void Init(AttackController _attackController)
    {
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        PlayerProjectile prefab = Resources.Load<PlayerProjectile>("PlayerProjectile");
        PlayerProjectile projectile = PoolManager.Instance.Spawn(prefab);
        projectile.transform.position = attackController.transform.position;
        projectile.Launch(targetPos, damage, true);

        // TODO:
        Debug.Log("멀티 타겟 원거리 공격");
    }

    public void ShowRange()
    {
        
    }
}
