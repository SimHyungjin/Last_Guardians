using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicProjectile : ProjectileBase
{
    public BaseMonster target;
    [SerializeField]private bool hasHit = false;
    public override void Init(TowerData _towerData, List<int> _effectslist)
    {
        base.Init(_towerData,_effectslist);
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/Magics/{spritename}.png";
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
                target.TakeDamage(towerData.AttackPower);
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
                    if (TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance < 1.0f) effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]), TowerManager.Instance.GetTowerData(effectslist[i]).EffectChance);
                    else effects[i].Apply(target, TowerManager.Instance.GetTowerData(effectslist[i]));
                    Debug.Log($"이펙트 적용 {TowerManager.Instance.GetTowerData(effectslist[i]).SpecialEffect}");
                }
            }
            OnDespawn();
            PoolManager.Instance.Despawn<MagicProjectile>(this);
        }
    }

}
