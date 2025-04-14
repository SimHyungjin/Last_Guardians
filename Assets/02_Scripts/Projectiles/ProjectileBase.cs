using JetBrains.Annotations;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface IProjectile
{
    void Launch(Vector2 targetPos);
}

public abstract class ProjectileBase : MonoBehaviour, IPoolable, IProjectile
{
    protected float speed = 10f;
    protected TowerData towerData;
    protected float Range = 5f;
    protected float offset = 0.5f;

    protected Vector2 direction;
    protected Vector2 startPos;

    protected Coroutine lifeTimeCoroutine;

    protected Rigidbody2D rb;

    //protected virtual void Awake()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //}

    public virtual void Update()
    {
        float distance = Vector2.Distance(transform.position, startPos);

        if (distance > Range+offset)
        {
            PoolManager.Instance.Despawn(this);
        }
    }
    public virtual void Init(TowerData _towerData) 
    {
        rb = GetComponent<Rigidbody2D>();
        towerData = _towerData;
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
        //lifeTimeCoroutine = StartCoroutine(DespawnProjectile(lifeTime));
    }

    public void OnDespawn()
    {
        if (lifeTimeCoroutine != null)
        {
            //StopCoroutine(lifeTimeCoroutine);
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
    public virtual void Launch(Vector2 targetPos)
    {
        direction = (targetPos - (Vector2)transform.position).normalized;
        startPos = transform.position;
        transform.right = direction;

        //이후에 override로 동작구현
        ProjectileMove();
    }

    protected virtual void ProjectileMove()
    {
        rb.velocity = direction * speed;
    }
}


//IEnumerator DespawnProjectile(float time = 0)
//{
//    yield return new WaitForSeconds(time);
//    PoolManager.Instance.Despawn(this);
//    lifeTimeCoroutine = null;
//}
//충돌예시
/*
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
        effectIndicator.effectChangeSprite.ShowCircle("Effect/Circle", hitPoint, multiSpread);

        var hits = Physics2D.OverlapCircleAll(hitPoint, multiSpread * 0.5f, LayerMask.GetMask("Monster"));
        foreach (var hit in hits)
        {
            // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
            // TODO:
            Debug.Log("멀티 불릿 공격" + hit.name);
        }
    }
    else if (!hitTarget)
    {
        hitTarget = true;
        // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
        // TODO:
        Debug.Log("단일 불릿 공격");
    }
    PoolManager.Instance.Despawn(this);
}
*/
