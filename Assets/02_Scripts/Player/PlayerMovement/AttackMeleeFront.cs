using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackMeleeFront : IAttackBehavior
{
    private AttackController attackController;
    private Player player;

    public void Init(AttackController _attackController)
    {
        player = InGameManager.Instance.playerManager.player;
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        Vector2 forward = attackController.transform.right;
        float range = player.playerData.attackRange;
        float width = player.playerData.attackRange * 0.5f;

        Vector2 boxCenter = (Vector2)attackController.transform.position + forward * range;
        Vector2 boxSize = new Vector2(range * 2, width);

        float angle = attackController.transform.eulerAngles.z;

        var rawHits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, LayerMaskData.monster);

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
            hit.GetComponent<BaseMonster>().TakeDamage(damage);
            // TODO:
            Debug.Log("정면 근접 공격");
        }
    }
    public void ShowRange()
    {
        EffectIndicator prefab = Resources.Load<EffectIndicator>("Effect/EffectIndicator");
        EffectIndicator effectIndicator = PoolManager.Instance.Spawn(prefab);

        float width = player.playerData.attackRange * 0.5f;
        float range = player.playerData.attackRange;

        Vector2 forward = attackController.transform.right;
        Vector2 center = (Vector2)attackController.transform.position + forward * range;

        effectIndicator.effectChangeSprite.ShowSquare("Effect/Square", center, range * 2, width);
        effectIndicator.transform.rotation = attackController.transform.rotation;
    }
}
