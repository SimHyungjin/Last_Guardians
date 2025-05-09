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
        PlayerProjectile prefab = Resources.Load<PlayerProjectile>("Player/PlayerProjectile");
        PlayerProjectile projectile = PoolManager.Instance.Spawn(prefab);
        projectile.transform.position = (Vector2)attackController.transform.position;
        projectile.Launch(targetPos, damage);
    }
    public void ShowRange()
    {

    }
}
