using System.Collections.Generic;
using UnityEngine;

public class AttackMeleeCircle : IAttackBehavior
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
        var rawHits = Physics2D.OverlapCircleAll(attackController.transform.position, player.attackRange * 2, LayerMaskData.monster);

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
            Debug.Log("범위 근접 공격");
        }

    }
    public void ShowRange()
    {
        EffectIndicator prefab = Resources.Load<EffectIndicator>("Effect/EffectIndicator");
        EffectIndicator effectIndicator = PoolManager.Instance.Spawn(prefab);

        effectIndicator.effectChangeSprite.ShowCircle("Effect/Circle", attackController.transform.position, player.attackRange * 4);
    }
}
