using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public interface IAttackBehavior
{
    void Init(PlayerAttackController _controller);
    void Attack(Vector2 targetPos, float damage);
    void ShowRange();
}

/// <summary>
/// 이 스크립트는 캐릭터의 자동 공격을 담당합니다.
/// 공격 범위 내 가장 가까운 몬스터를 탐색하고, 정해진 딜레이로 순차적으로 공격합니다.
/// </summary>
public class PlayerAttackController : MonoBehaviour
{
    private Player player;
    private PlayerWeaponController weaponHandler;
    private GameObject target;
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    private const float targetCheckTime = 0.05f;
    private Vector2? currentTargetPos = null;

    private IAttackBehavior attackBehavior;
    /// <summary>
    /// 캐릭터 데이터를 주입하고 자동 공격을 시작합니다.
    /// </summary>
    public void Init()
    {
        player = InGameManager.Instance.playerManager.playerHandler.player;
        weaponHandler = InGameManager.Instance.playerManager.playerHandler.weaponHandler;
        SetAttackBehavior(player.playerData.attackType);
        AutoAttackStart();
        if (weaponHandler.attackAction != null) weaponHandler.attackAction -= Attack;
        weaponHandler.attackAction += Attack;
    }

    /// <summary>
    /// 자동 공격을 시작합니다.
    /// </summary>
    public void AutoAttackStart()
    {
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(FindEnemy());
    }

    /// <summary>
    /// 자동 공격을 중단합니다.
    /// </summary>
    public void AutoAttackStop()
    {
        if (attackCoroutine == null) return;
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
    }

    /// <summary>
    /// 공격 범위 내에서 가장 가까운 적을 탐색하고, 회전 및 공격을 수행합니다.
    /// </summary>
    private IEnumerator FindEnemy()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Monster");

        while (true)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, player.playerData.attackRange * 2, layerMask);

            if (hits.Length == 0)
            {
                //Debug.LogWarning("몬스터를 찾지 못하였습니다");
            }
            else
            {
                float minDistance = float.MaxValue;

                foreach (var hit in hits)
                {
                    if (hit == null || !hit.gameObject.activeInHierarchy) continue;

                    float distance = Vector2.Distance(transform.position, hit.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = hit.gameObject;
                    }
                }

                if (target != null && !isAttacking)
                {
                    currentTargetPos = target.transform.position;
                    weaponHandler.CallAttackEnter((Vector2)currentTargetPos);
                    target = null;
                    yield return new WaitForSeconds(player.playerData.attackSpeed);
                }
            }

            yield return new WaitForSeconds(targetCheckTime);
        }
    }

    /// <summary>
    /// 공격 형태를 변경합니다.
    /// </summary>
    /// 
    public void SetAttackBehavior(IAttackBehavior behavior)
    {
        attackBehavior = behavior;
        behavior.Init(this);
    }

    public void SetAttackBehavior(AttackType attackType)
    {
        IAttackBehavior behavior = attackType switch
        {
            AttackType.Melee => new AttackMeleeFan(),
            AttackType.Ranged => new AttackRangedSingle(),
            AttackType.Area => new AttackRangedMulti(),
            _ => new AttackMeleeFan()
        };
        SetAttackBehavior(behavior);
    }

    /// <summary>
    /// 공격을 수행하며, 감지된 몬스터를 거리순으로 정렬 후 처리합니다.
    /// </summary>
    private void Attack()
    {
        if (currentTargetPos == null || isAttacking) return;

        isAttacking = true;
        attackBehavior.Attack(currentTargetPos.Value, CalculateDamage());
        attackBehavior.ShowRange();
        StartCoroutine(AttackDelay());

        currentTargetPos = null;
    }

    /// <summary>
    /// 공격 후 딜레이 코루틴입니다.
    /// </summary>
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(player.playerData.attackSpeed);
        isAttacking = false;
    }

    /// <summary>
    /// 데미지를 계산합니다. 크리티컬이 터질 경우 배수 적용됩니다.
    /// </summary>
    private float CalculateDamage()
    {
        float damage = player.playerData.attackPower;
        float randomFloat = Random.Range(0, 100);

        if (randomFloat <= player.playerData.criticalChance)
            damage *= player.playerData.criticalDamage;

        return damage;
    }

    /// <summary>
    /// 공격 범위 뷰에서 시각화합니다.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (player == null) return; 

        // 감지 범위 (빨간 원)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, player.playerData.attackRange * 2);

        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // 반투명 빨간색
    }
}
