using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ArrowProjectile : ProjectileBase
{
    public BaseMonster target;
    [SerializeField] private bool hasHit = false;

    /// <summary>
    /// 타워의 데이터와 적응된 타워의 데이터, 이펙트 리스트, 환경 이펙트를 초기화합니다.
    /// </summary>
    /// <param name="_towerData"></param>
    /// <param name="_adaptedTowerData"></param>
    /// <param name="_effectslist"></param>
    /// <param name="_environmentEffect"></param>
    public override void Init(TowerData _towerData,AdaptedTowerData _adaptedTowerData,List<int> _effectslist, EnvironmentEffect _environmentEffect)
    {
        base.Init(_towerData, _adaptedTowerData, _effectslist,_environmentEffect);
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/Arrows/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;
#endif
    }
    public override void Update()
    {
        base.Update();
        float distance = Vector2.Distance(transform.position, startPos);

        if (distance > Range + offset)
        {
            PoolManager.Instance.Despawn<ArrowProjectile>(this);
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
        effects.Clear();
    }

    /// <summary>
    /// 충돌시 데미지를주고, 저장된 이펙트를 적용
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
         
            target.TakeDamage(adaptedTower.attackPower);
            //이펙트적용부분
            if (effects == null)
            {
                OnDespawn();
                PoolManager.Instance.Despawn<ArrowProjectile>(this);
                return;
            }
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i] == null) continue;
                if (TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance < 1.0f) effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]),adaptedTower ,TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance,environmentEffect);
                else effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]), adaptedTower,environmentEffect);

            }

            OnDespawn();
            PoolManager.Instance.Despawn<ArrowProjectile>(this);

        }
    }

}
