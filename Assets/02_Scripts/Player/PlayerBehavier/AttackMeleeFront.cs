using System.Collections.Generic;
using UnityEngine;

public class AttackMeleeFront : IAttackBehavior
{
    private PlayerAttackController attackController;
    private PlayerStatus player;

    public void Init(PlayerAttackController _attackController)
    {
        player = GameManager.Instance.PlayerManager.playerStatus;
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        Vector2 forward = attackController.transform.right;
        float range = player.attackRange;
        float width = player.attackRange * 0.5f;

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
        }
    }
    public void ShowRange()
    {
        EffectIndicator prefab = Resources.Load<EffectIndicator>("Effect/EffectIndicator");
        EffectIndicator effectIndicator = PoolManager.Instance.Spawn(prefab);

        float width = player.attackRange * 0.5f;
        float range = player.attackRange;

        Vector2 forward = attackController.transform.right;
        Vector2 center = (Vector2)attackController.transform.position + forward * range;

        effectIndicator.effectChangeSprite.ShowSquare("Effect/Square", center, range * 2, width);
        effectIndicator.transform.rotation = attackController.transform.rotation;
    }
}
