using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicProjectile : ProjectileBase
{
    public BaseMonster target;
    [SerializeField] private bool hasHit = false;
    private Animator animator;
    /// <summary>
    /// 타워의 데이터와 적응된 타워의 데이터, 이펙트 리스트, 환경 이펙트를 초기화합니다.
    /// </summary>
    /// <param name="_towerData"></param>
    /// <param name="adaptedTower"></param>
    /// <param name="_effectslist"></param>
    /// <param name="_environmentEffect"></param>
    public override void Init(TowerData _towerData, AdaptedTowerData adaptedTower, List<int> _effectslist, EnvironmentEffect _environmentEffect)
    {
        base.Init(_towerData, adaptedTower, _effectslist, _environmentEffect);
        GetComponent<SpriteRenderer>().enabled = true;
        Utils.GetColor(towerData, GetComponent<SpriteRenderer>());
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        float distance = Vector2.Distance(transform.position, startPos);

        if (distance > towerData.AttackRange / 2 + offset)
        {
            PoolManager.Instance.Despawn<MagicProjectile>(this);
        }
    }
    protected override void ProjectileMove()
    {
        rb.velocity = direction * speed;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        hasHit = false;
        rb.velocity = Vector2.zero;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        target = null;
        //effect = null;
        //effects.Clear();
    }

    /// <summary>
    /// 충돌시 데미지와 이펙트적용
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            BaseMonster target = collision.GetComponent<BaseMonster>();

            if (OriginTarget == target)
                return;

            hasHit = true;

            if (target != null)
            {
                target.TakeDamage(adaptedTower.attackPower);
                //이펙트적용부분
                if (effects == null)
                {
                    OnDespawn();
                    PoolManager.Instance.Despawn<MagicProjectile>(this);
                    return;
                }
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i] == null) continue;
                    if (TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance < 1.0f) effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]), adaptedTower, TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance, environmentEffect);
                    else effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]), adaptedTower, environmentEffect);
                }
            }
            OnDespawn();
            animator.SetTrigger("isHit");
            StartCoroutine(WaitAndDespawn());
            //PoolManager.Instance.Despawn<MagicProjectile>(this);
        }
    }
    /// <summary>
    /// 현재 애니메이션(폭발)의 길이를 가져와 대기후 Despawn  
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndDespawn()
    {
        yield return null;
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength-0.01f);
        GetComponent<SpriteRenderer>().enabled = false;
        PoolManager.Instance.Despawn<MagicProjectile>(this);
    }
}
