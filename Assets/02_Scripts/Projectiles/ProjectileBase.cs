using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface IProjectile
{
    void Launch(Vector2 targetPos);
}

public abstract class ProjectileBase : MonoBehaviour, IPoolable, IProjectile
{
    public List<IEffect> effects;// 나중에 다중 이펙트 처리할때 사용
    public List<int> effectslist;
    public IEffect effect;
    protected float speed = 5f;
    protected TowerData towerData;
    protected float Range = 5f;
    protected float offset = 0.5f;

    protected Vector2 direction;
    protected Vector2 startPos;
    protected Vector2 targetPos;
    protected Coroutine lifeTimeCoroutine;
    protected Rigidbody2D rb;

    public BaseMonster OriginTarget { get; set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {
        //float distance = Vector2.Distance(transform.position, startPos);

        //if (distance > Range + offset)
        //{
        //    PoolManager.Instance.Despawn(this);
        //}
    }
    public virtual void Init(TowerData _towerData,List<int> _effectslist) 
    {
        towerData = _towerData;
        effectslist = _effectslist;
    }
    public virtual void OnSpawn()
    {
        if (rb != null)
           rb.velocity = Vector2.zero;
        OriginTarget = null;
    }

    public virtual void OnDespawn()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;
        OriginTarget = null;
    }
    /// <summary>
    /// damage,isMulti 초기화, targetPos를 향해 회전 후 발사
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="_damage"></param>
    /// <param name="_isMulti"></param>
    public virtual void Launch(Vector2 _targetPos)
    {
        targetPos = _targetPos;
        direction = (targetPos - (Vector2)transform.position).normalized;
        startPos = transform.position;
        transform.right = direction;
        //이후에 override로 동작구현
        ProjectileMove();
    }

    protected virtual void ProjectileMove(){}
}

