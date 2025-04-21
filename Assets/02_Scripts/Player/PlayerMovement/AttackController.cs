using System.Collections;
using UnityEngine;

public interface IAttackBehavior
{
    void Init(AttackController _controller);
    void Attack(Vector2 targetPos, float damage);
    void ShowRange();
}

/// <summary>
/// 이 스크립트는 캐릭터의 자동 공격을 담당합니다.
/// 공격 범위 내 가장 가까운 몬스터를 탐색하고, 정해진 딜레이로 순차적으로 공격합니다.
/// </summary>
public class AttackController : MonoBehaviour
{
    public  Player player {  get; private set; }
    private GameObject target;
    private Coroutine attackCoroutine;
    private bool isAttacking = false;
    private const float targetCheckTime = 0.05f;

    private IAttackBehavior attackBehavior;
    /// <summary>
    /// 캐릭터 데이터를 주입하고 자동 공격을 시작합니다.
    /// </summary>
    public void Init(Player _player)
    {
        player = _player;
        Debug.Log(player.playerData.attackType);
        SetAttackBehavior(player.playerData.attackType);
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
    private void Attack(Vector2 targetPosition)
    {
        isAttacking = true;
        attackBehavior.Attack(targetPosition, CalculateDamage());
        attackBehavior.ShowRange();
        StartCoroutine(AttackDelay());
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
    /// 공격 범위 및 타격 박스를 Scene 뷰에서 시각화합니다.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (player == null) return;

        // 감지 범위 (빨간 원)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, player.playerData.attackRange * 2);

        Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // 반투명 빨간색

        Vector2 forward = transform.right;
        float range = player.playerData.attackRange;
        float width = player.playerData.attackRange * 0.5f; // AttackController에 public float rangeX;

        Vector2 boxCenter = (Vector2)transform.position + forward * range;
        Vector2 boxSize = new Vector2(width, range);
        float angle = transform.eulerAngles.z;

        // 회전된 박스를 제대로 그리기 위한 행렬 설정
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        // 매트릭스 초기화 (다음 Gizmos에 영향 안 주게)
        Gizmos.matrix = Matrix4x4.identity;
    }
}
