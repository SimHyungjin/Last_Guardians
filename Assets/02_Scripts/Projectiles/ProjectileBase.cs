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
    //private float speed = 10f;
    private float Range = 5f;
    private float offset = 0.5f;

    private Vector2 direction;
    private Vector2 startPos;

    private Coroutine lifeTimeCoroutine;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        float distance = Vector2.Distance(transform.position, startPos);

        if (distance > Range+offset)
        {
            PoolManager.Instance.Despawn(this);
        }
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
    /// damage,isMulti �ʱ�ȭ, targetPos�� ���� ȸ�� �� �߻�
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="_damage"></param>
    /// <param name="_isMulti"></param>
    public virtual void Launch(Vector2 targetPos)
    {
        direction = (targetPos - (Vector2)transform.position).normalized;
        startPos = transform.position;
        transform.right = direction;

        //���Ŀ� override�� ���۱���
        ProjectileMove();
    }

    public virtual void ProjectileMove()
    {

        //rb.velocity = direction * speed;
    }
}


//IEnumerator DespawnProjectile(float time = 0)
//{
//    yield return new WaitForSeconds(time);
//    PoolManager.Instance.Despawn(this);
//    lifeTimeCoroutine = null;
//}
//�浹����
/*
/// <summary>
/// isMulti �� ���� ���� ����/1�� ���� ó��
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
            // ������ ó�� �� ����Ʈ�� ���� ������ ���� ����
            // TODO:
            Debug.Log("��Ƽ �Ҹ� ����" + hit.name);
        }
    }
    else if (!hitTarget)
    {
        hitTarget = true;
        // ������ ó�� �� ����Ʈ�� ���� ������ ���� ����
        // TODO:
        Debug.Log("���� �Ҹ� ����");
    }
    PoolManager.Instance.Despawn(this);
}
*/
