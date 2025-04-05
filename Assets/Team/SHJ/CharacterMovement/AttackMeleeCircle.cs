using System.Linq;
using UnityEngine;

public class AttackMeleeCircle : IAttackBehavior
{
    private AttackController controller;

    public void Init(AttackController _controller)
    {
        controller = _controller;
    }

    public void Attack(Vector2 targetPos, float damage)
    {   
        var hits = Physics2D.OverlapCircleAll(controller.transform.position, controller.character.player.attackRange, LayerMask.GetMask("Monster"))
            .Where(h => h != null && h.gameObject.activeInHierarchy)
            .OrderBy(h => Vector2.Distance(controller.transform.position, h.transform.position))
            .ToArray();

        foreach (var hit in hits)
        {
            // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
            Debug.Log("범위 근접 공격");
        }

    }
}
