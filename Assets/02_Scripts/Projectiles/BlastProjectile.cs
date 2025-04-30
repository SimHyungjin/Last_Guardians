using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BlastProjectile : ProjectileBase
{
    public BlastZone blastEffect;
    private BlastZone blastEffectInstance;

    public BaseMonster target;
    private Tweener moveTween;

    private float ExplosionRadius = 1f; // ���� �ݰ�
    private bool canHit = false;
    private float Totaldistance;
    private bool hasHit = false;

    /// <summary>
    /// Ÿ���� �����Ϳ� ������ Ÿ���� ������, ȿ�� ����Ʈ, ȯ�� ȿ���� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="_towerData"></param>
    /// <param name="_adaptedTowerData"></param>
    /// <param name="_effectslist"></param>
    /// <param name="_environmentEffect"></param>
    public override void Init(TowerData _towerData, AdaptedTowerData _adaptedTowerData,List<int> _effectslist, EnvironmentEffect _environmentEffect)
    {
        base.Init(_towerData, _adaptedTowerData, _effectslist, _environmentEffect);
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

        if (!canHit && ratio >= 0.7f)
        {
            Debug.Log("canHit");
            canHit = true;
        }
    }

    /// <summary>
    /// ������ ������ ���� �߻�
    /// ����,�����ʱ����Ͽ� �׻� �������� �ְ�
    /// ���������� �����ϸ� ����
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
    /// �߻����Ŀ� ���� �Ÿ� �̻� �̵����� �� �浹����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit&&!canHit) return;
        hasHit = true;
        canHit = false;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            hasHit = true;
            BaseMonster target = collision.GetComponent<BaseMonster>();
            moveTween.Kill();
            target.TakeDamage(adaptedTower.attackPower);
            Explode();
        }
    }

    /// <summary>
    /// ���߽� ������ ���Ϳ��� �������� ����Ʈ ����
    /// </summary>
    private void Explode()
    {
        Debug.Log("����");
        blastEffectInstance = PoolManager.Instance.Spawn<BlastZone>(blastEffect);
        blastEffectInstance.Init(towerData, this.transform);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius/2, LayerMaskData.monster);
        int count = 0;
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                count++;
                if (effects == null) break;
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i] == null) continue;
                    if (TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance < 1.0f) effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]),adaptedTower ,TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance, environmentEffect);
                    else effects[i].Apply(monster, TowerManager.Instance.GetTowerData(effectslist[i]), adaptedTower, environmentEffect);
                }
            }
        }
        Debug.Log($"���߹����ȿ� ���� {count}");
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
        if(effects!=null) effects.Clear();
    }  
}
