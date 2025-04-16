using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BlastProjectile : ProjectileBase
{
    public BlastZone blastEffect;

    public BaseMonster target;
    private float arcHeight = 0.4f;
    private float duration = 0.5f;
    private Tweener moveTween;
    private float ExplosionRadius = 1f; // 폭발 반경
    private bool canHit = false;
    private float Totaldistance;
    [SerializeField] private bool hasHit = false;
    public override void Init(TowerData _towerData, List<int> _effectslist)
    {
        base.Init(_towerData,_effectslist);
        Totaldistance = Vector2.Distance(startPos, targetPos);
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        speed = 2f;
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/Blasts/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;
#endif
    }

    public override void Update()
    {
        base.Update();
        float traveled = Vector2.Distance(startPos, transform.position);
        float ratio = traveled / Totaldistance;

        if (!canHit && ratio >= 0.8f)
        {
            Debug.Log("canHit");
            canHit = true;
        }
    }

    protected override void ProjectileMove()
    {
        Vector2 start = transform.position;
        Vector2 dir = (targetPos - start).normalized;
        //float distance = Vector2.Distance(start, targetPos);
        float duration = Totaldistance / speed;

        float arcHeight = 0.4f;

        Vector2 mid = Vector2.Lerp(start, targetPos, 0.5f);

        Vector2 perp = Vector2.Perpendicular(dir).normalized;
        if (dir.x < 0) perp *= -1;
        Vector2 controlPoint = mid + perp * arcHeight;

        Vector3[] path = new Vector3[]
        {
        start,
        controlPoint,
        targetPos
        };

        moveTween = transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("바닥에서터짐");
                Explode();
            });
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit&&!canHit) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("몬스터와 충돌");
            hasHit = true;
            BaseMonster target = collision.GetComponent<BaseMonster>();
            moveTween.Kill();
            target.TakeDamage(towerData.AttackPower);
            Explode();
        }
    }

    private void Explode()
    {
        //터지면서 주변에 장판 생성,주변에 OverlapCircleAll 범위 1f만큼 효과부여

        //if (towerData.SpecialEffect == SpecialEffect.None || effect == null)
        //{
        //    OnDespawn();
        //    PoolManager.Instance.Despawn<ProjectileBase>(this);
        //    return;
        //}
        //if (towerData.EffectChance < 1.0f) effect.Apply(target, towerData, towerData.EffectChance);
        //else effect.Apply(target, towerData);
        OnDespawn();
        PoolManager.Instance.Despawn<BlastProjectile>(this);
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
        effects.Clear();
    }
    
}
