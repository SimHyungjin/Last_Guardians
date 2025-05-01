using System.Collections.Generic;
using UnityEngine;

public class AttackMeleeFan : IAttackBehavior
{
    private PlayerAttackController attackController;
    private Player player;

    float angle;
    float radius;
    Vector2 targetPos;

    public void Init(PlayerAttackController _attackController)
    {
        player = InGameManager.Instance.playerManager.player;
        attackController = _attackController;
    }

    public void Attack(Vector2 targetPos, float damage)
    {
        this.targetPos = targetPos;
        Vector2 origin = attackController.transform.position;
        Vector2 direction = (targetPos - origin).normalized;

        angle = 90f;
        radius = player.playerData.attackRange * 2;
        var rawHits = Physics2D.OverlapCircleAll(origin, radius, LayerMaskData.monster);

        List<Collider2D> validHits = new();

        foreach (var hit in rawHits)
        {
            if (hit != null && hit.gameObject.activeInHierarchy)
            {
                Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;

                float dot = Vector2.Dot(direction, toTarget);
                dot = Mathf.Clamp(dot, -1f, 1f);
                float angleToTarget = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angleToTarget <= angle)
                {
                    validHits.Add(hit);
                }
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

        Vector2 direction = (targetPos -(Vector2)attackController.transform.position).normalized;
        effectIndicator.effectChangeMesh.ShowFan(attackController.transform.position, direction, player.playerData.attackRange * 4, angle);
    }
}
