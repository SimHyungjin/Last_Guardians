using System.Linq;
using UnityEngine;

public class AttackMeleeFront : IAttackBehavior
{
    private AttackController attackController;

    public void Init(AttackController _attackController)
    {
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        Vector2 forward = attackController.transform.right;
        Vector2 boxCenter = (Vector2)attackController.transform.position + forward * attackController.player.playerData.attackRange;
        Vector2 boxSize = new Vector2(2f, attackController.player.playerData.attackRange);

        var hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, LayerMask.GetMask("Monster"))
            .Where(h => h != null && h.gameObject.activeInHierarchy)
            .OrderBy(h => Vector2.Distance(attackController.transform.position, h.transform.position))
            .ToArray();

        foreach (var hit in hits)
        {
            // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
            // TODO:
            Debug.Log("정면 근접 공격");
        }

    }
}
