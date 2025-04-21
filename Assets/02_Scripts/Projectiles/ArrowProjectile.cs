using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArrowProjectile : ProjectileBase
{
    public BaseMonster target;
    [SerializeField] private bool hasHit = false;
    public override void Init(TowerData _towerData,AdaptedTowerData _adaptedTowerData,List<int> _effectslist)
    {
        base.Init(_towerData, _adaptedTowerData, _effectslist);
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
                if (TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance < 1.0f) effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]),adaptedTower ,TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance);
                else effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]), adaptedTower);

                Debug.Log($"이펙트 적용 {effects[i].GetType()}");
                Debug.Log($"이펙트 적용 {TowerManager.Instance.GetTowerData(effectslist[i]).SpecialEffect}");
            }
            //if (towerData.EffectChance < 1.0f) effect.Apply(target, towerData, towerData.EffectChance);
            //else effect.Apply(target, towerData);

            OnDespawn();
            PoolManager.Instance.Despawn<ArrowProjectile>(this);

        }
    }

}
