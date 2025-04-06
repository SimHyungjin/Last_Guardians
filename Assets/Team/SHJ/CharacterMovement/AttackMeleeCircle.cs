using System.Linq;
using UnityEngine;

public class AttackMeleeCircle : IAttackBehavior
{
    private AttackController attackController;

    public void Init(AttackController _attackController)
    {
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {   
        var hits = Physics2D.OverlapCircleAll(attackController.transform.position, attackController.player.playerData.attackRange, LayerMask.GetMask("Monster"))
            .Where(h => h != null && h.gameObject.activeInHierarchy)
            .OrderBy(h => Vector2.Distance(attackController.transform.position, h.transform.position))
            .ToArray();

        foreach (var hit in hits)
        {
            // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
            // TODO:
            Debug.Log("범위 근접 공격");
        }

    }
    public void ShowRange()
    {
        EffectIndicator prefab = Resources.Load<EffectIndicator>("Effect/EffectIndicator");
        EffectIndicator effectIndicator = PoolManager.Instance.Spawn(prefab);

        effectIndicator.ChangeSpriteCircle("Effect/Circle", attackController.transform.position, attackController.player.playerData.attackRange * 4);
    }
}
