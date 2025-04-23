using UnityEngine;

public class AttackRangedSingle : IAttackBehavior
{
    private PlayerAttackController attackController;

    public void Init(PlayerAttackController _attackController)
    {
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        PlayerProjectile prefab = Resources.Load<PlayerProjectile>("PlayerProjectile");
        PlayerProjectile projectile = PoolManager.Instance.Spawn(prefab);
        projectile.transform.position = attackController.transform.position;
        projectile.Launch(targetPos, damage);

        // TODO:
        Debug.Log("단일 타겟 원거리 공격");
    }
    public void ShowRange()
    {

    }
}
