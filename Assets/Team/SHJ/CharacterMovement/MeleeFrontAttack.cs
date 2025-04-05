using System.Linq;
using UnityEngine;

public class MeleeFrontAttack : IAttackBehavior
{
    private AttackController controller;

    public void Init(AttackController _controller)
    {
        controller = _controller;
    }

    public void Attack(Vector2 targetPos)
    {
        Vector2 forward = controller.transform.right;
        Vector2 boxCenter = (Vector2)controller.transform.position + forward * controller.character.player.attackRange;
        Vector2 boxSize = new Vector2(2f, controller.character.player.attackRange);

        var hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, LayerMask.GetMask("Monster"))
            .Where(h => h != null && h.gameObject.activeInHierarchy)
            .OrderBy(h => Vector2.Distance(controller.transform.position, h.transform.position))
            .ToArray();

    }
}
