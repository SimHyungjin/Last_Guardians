using System.Collections.Generic;
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
        var rawHits = Physics2D.OverlapCircleAll(attackController.transform.position, attackController.player.playerData.attackRange * 2, LayerMask.GetMask("Monster"));

        List<Collider2D> validHits = new();
        foreach (var hit in rawHits)
        {
            if (hit != null && hit.gameObject.activeInHierarchy)
            {
                validHits.Add(hit);
            }
        }
        validHits.Sort((a, b) => Vector2.Distance(attackController.transform.position, a.transform.position).CompareTo(Vector2.Distance(attackController.transform.position, b.transform.position)));

        foreach (var hit in validHits)
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
