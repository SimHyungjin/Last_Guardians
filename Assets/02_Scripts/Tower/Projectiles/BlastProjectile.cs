using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BlastProjectile : ProjectileBase
{
    public BlastZone blastEffect;
    private BlastZone blastEffectInstance;

    public BaseMonster target;
    private Tweener moveTween;

    private float explosionRadius; // 폭발 반경
    private bool canHit = false;
    private float Totaldistance;
    private bool hasHit = false;
    /// <summary>
    /// 타워의 데이터와 적응된 타워의 데이터, 효과 리스트, 환경 효과를 초기화합니다.
    /// </summary>
    /// <param name="_towerData"></param>
    /// <param name="_adaptedTowerData"></param>
    /// <param name="_effectslist"></param>
    /// <param name="_environmentEffect"></param>
    public override void Init(TowerData _towerData, AdaptedAttackTowerData _adaptedTowerData, List<int> _effectslist, EnvironmentEffect _environmentEffect)
    {
        base.Init(_towerData, _adaptedTowerData, _effectslist, _environmentEffect);
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        GetComponent<SpriteRenderer>().sprite = projectileAtlas.GetSprite(spritename);
        explosionRadius = 1f * TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectRange].levels[TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectRange]];

    }

    public override void Update()
    {
        base.Update();
        float traveled = Vector2.Distance(startPos, transform.position);
        float ratio = traveled / Totaldistance;

        if (!canHit && ratio >= 0.7f)
        {
            canHit = true;
        }
    }

    /// <summary>
    /// 포물선 궤적을 따라 발사
    /// 왼쪽,오른쪽구분하여 항상 위쪽으로 휘게
    /// 도착지점에 도착하면 폭발
    /// </summary>
    protected override void ProjectileMove()
    {
        Totaldistance = Vector2.Distance(startPos, targetPos);
        Vector2 start = transform.position;
        Vector2 dir = (targetPos - start).normalized;
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
                Explode();
            });
    }

    /// <summary>
    /// 발사이후에 일정 거리 이상 이동했을 때 충돌가능
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit && !canHit) return;
        canHit = false;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            hasHit = true;
            BaseMonster target = collision.GetComponent<BaseMonster>();
            moveTween.Kill();
            target.TakeDamage(adaptedTower.attackPower, penetration);
            Explode();
        }
    }

    /// <summary>
    /// 폭발시 범위내 몬스터에게 데미지와 이펙트 적용
    /// </summary>
    private void Explode()
    {
        SoundManager.Instance.PlaySFX("HitBlast");
        blastEffectInstance = PoolManager.Instance.Spawn<BlastZone>(blastEffect);
        blastEffectInstance.Init(towerData, this.transform);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius / 2, LayerMaskData.monster);
        int count = 0;
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                count++;
                if (effects == null) break;
                if(towerData.SpecialEffect==SpecialEffect.ChainAttack)monster.TakeDamage(adaptedTower.attackPower*adaptedTower.effectValue, penetration);
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i] == null) continue;
                    if (i >= 0 && i < effectslist.Count && i < effects.Count)
                    {
                        if (TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance < 1.0f)
                        {
                            effects[i].Apply(target,
                                             TowerManager.Instance.GetTowerData(effectslist[i]),
                                             TowerManager.Instance.GetAdaptedAttackTowerData(effectslist[i]),
                                             TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance,
                                             environmentEffect);
                        }
                        else
                        {
                            effects[i].Apply(monster,
                                             TowerManager.Instance.GetTowerData(effectslist[i]),
                                             TowerManager.Instance.GetAdaptedAttackTowerData(effectslist[i]),
                                             environmentEffect);
                        }
                    }
                    else
                    {
                        Debug.LogError("Index out of range: " + i);
                    }
                }
            }
        }
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
        if (effects != null) effects.Clear();
    }
}
