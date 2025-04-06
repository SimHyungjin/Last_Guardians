using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour,IPoolable
{
    private float speed = 10f;
    private float lifeTime = 5f;
    private float multiSpread = 1f;

    private bool hitTarget;

    private float damage;
    private Vector2 direction;

    private bool isMulti = false;

    private Coroutine lifeTimeCoroutine;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnSpawn()
    {
        if (lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
        if (rb != null)
            rb.velocity = Vector2.zero;
        lifeTimeCoroutine = StartCoroutine(DespawnProjectile(lifeTime));
        hitTarget = false;
    }

    public void OnDespawn()
    {
        if (lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
    /// <summary>
    /// damage,isMulti 초기화, targetPos를 향해 회전 후 발사
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="_damage"></param>
    /// <param name="_isMulti"></param>
    public void Launch(Vector2 targetPos, float _damage, bool _isMulti = false)
    {
        damage = _damage;
        isMulti = _isMulti;
        direction = (targetPos - (Vector2)transform.position).normalized;
        transform.right = direction;

        rb.velocity = direction * speed;
    }
    /// <summary>
    /// time후에 투사체 제거
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DespawnProjectile(float time = 0)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Despawn(this);
        lifeTimeCoroutine = null;
    }
    /// <summary>
    /// isMulti 의 값에 따라 범위/1인 공격 처리
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & (1 << LayerMask.NameToLayer("Monster"))) == 0) return;

        if (isMulti)
        {
            EffectIndicator prefab = Resources.Load<EffectIndicator>("Effect/EffectIndicator");
            EffectIndicator effectIndicator = PoolManager.Instance.Spawn(prefab);

            Vector2 hitPoint = collision.ClosestPoint(transform.position);
            effectIndicator.ChangeSpriteCircle("Effect/Circle", hitPoint, multiSpread);

            var hits = Physics2D.OverlapCircleAll(hitPoint, multiSpread * 0.5f, LayerMask.GetMask("Monster"));
            foreach (var hit in hits)
            {
                // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
                // TODO:
                Debug.Log("멀티 불릿 공격" + hit.name);
            }
        }
        else if(!hitTarget)
        {
            hitTarget = true;
            // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
            // TODO:
            Debug.Log("단일 불릿 공격");
        }
        PoolManager.Instance.Despawn(this);
    }
}
