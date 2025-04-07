using System.Collections.Generic;
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
        float range = attackController.player.playerData.attackRange;
        float width = attackController.rangeX;

        Vector2 boxCenter = (Vector2)attackController.transform.position + forward * range;
        Vector2 boxSize = new Vector2(width, range);

        float angle = attackController.transform.eulerAngles.z;

        var rawHits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, LayerMask.GetMask("Monster"));

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
            Debug.Log("정면 근접 공격");
        }
    }
    public void ShowRange()
    {
        EffectIndicator prefab = Resources.Load<EffectIndicator>("Effect/EffectIndicator");
        EffectIndicator effectIndicator = PoolManager.Instance.Spawn(prefab);

        float width = attackController.rangeX;
        float height = attackController.player.playerData.attackRange;

        Vector2 forward = attackController.transform.right;
        Vector2 center = (Vector2)attackController.transform.position + forward * height;

        effectIndicator.ChangeSpriteSquare("Effect/Square", center, width, height);
        effectIndicator.transform.rotation = attackController.transform.rotation;
    }
}
