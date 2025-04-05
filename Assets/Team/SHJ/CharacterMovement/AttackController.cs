using System.Collections;
using UnityEngine;

public interface IAttackBehavior
{
    void Init(AttackController _controller);
    void Attack(Vector2 targetPos, float damage);
}

/// <summary>
/// 이 스크립트는 캐릭터의 자동 공격을 담당합니다.
/// 공격 범위 내 가장 가까운 몬스터를 탐색하고, 정해진 딜레이로 순차적으로 공격합니다.
/// </summary>
public class AttackController : MonoBehaviour
{
    public  Character character {  get; private set; }
    private GameObject target;
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    private const float targetCheckTime = 0.2f;

    private IAttackBehavior attackBehavior;
    /// <summary>
    /// 캐릭터 데이터를 주입하고 공격 모드를 근거리 정면 공격으로 변경합니다.
    /// 자동 공격을 시작합니다.
    /// </summary>
    public void Init(Character _character)
    {
        character = _character;
        AutoAttackStart();
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
            var hits = Physics2D.OverlapCircleAll(transform.position, character.player.attackRange * 2, layerMask);

            if (hits.Length == 0)
            {
                Debug.LogWarning("몬스터를 찾지 못하였습니다");
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

                if (target != null)
                {
                    // 타겟 방향으로 회전
                    Vector2 direction = (target.transform.position - transform.position).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);

                    Attack(target.transform.position);
                    target = null;
                }
            }

            yield return new WaitForSeconds(targetCheckTime);
        }
    }

    /// <summary>
    /// 공격 형태를 변경합니다.
    /// </summary>
    public void SetAttackBehavior(IAttackBehavior behavior)
    {
        attackBehavior = behavior;
        behavior.Init(this);
    }

    /// <summary>
    /// 공격을 수행하며, 감지된 몬스터를 거리순으로 정렬 후 처리합니다.
    /// </summary>
    private void Attack(Vector2 targetPosition)
    {
        if (isAttacking) return;
        isAttacking = true;

        if (attackBehavior == null)
            SetAttackBehavior(new AttackMeleeFront());
        attackBehavior.Attack(targetPosition, CalculateDamage());
        StartCoroutine(AttackDelay());
    }

    /// <summary>
    /// 공격 후 딜레이 코루틴입니다.
    /// </summary>
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(character.player.attackSpeed);
        isAttacking = false;
    }

    /// <summary>
    /// 데미지를 계산합니다. 크리티컬이 터질 경우 배수 적용됩니다.
    /// </summary>
    private float CalculateDamage()
    {
        float damage = character.player.attackPower;
        float randomFloat = Random.Range(0, 100);

        if (randomFloat <= character.player.criticalChance)
            damage *= character.player.criticalDamage;

        return damage;
    }

    /// <summary>
    /// 공격 범위 및 타격 박스를 Scene 뷰에서 시각화합니다.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (character == null) return;

        // 감지 범위 (빨간 원)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, character.player.attackRange * 2);

        // 공격 사각형 (초록 박스)
        Vector3 forward = transform.right;
        float distance = character.player.attackRange;
        Vector3 center = transform.position + forward * distance;
        Vector2 size = new Vector2(2f, character.player.attackRange);

        Gizmos.color = Color.green;
        Matrix4x4 rotMatrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
        Gizmos.matrix = rotMatrix;
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
