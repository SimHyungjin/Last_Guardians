using UnityEngine;

public class AttackSingleRanged : IAttackBehavior
{
    private AttackController controller;

    public void Init(AttackController _controller)
    {
        controller = _controller;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        CharacterProjectile projectile = Utils.InstantiateResource("CharacterProjectile").GetComponent<CharacterProjectile>();
        projectile.transform.position = controller.transform.position;
        projectile.Launch(targetPos, damage);

        Debug.Log("단일 타겟 원거리 공격");
    }
}
