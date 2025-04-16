using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicProjectile : ProjectileBase
{
    public BaseMonster target;
    [SerializeField]private bool hasHit = false;
    public override void Init(TowerData _towerData)
    {
        base.Init(_towerData);
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/Magics/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;
#endif
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
            hasHit = true;
            BaseMonster target = collision.GetComponent<BaseMonster>();
            target.TakeDamage(towerData.AttackPower);
            if (towerData.SpecialEffect == SpecialEffect.None || effect == null)
            {
                OnDespawn();
                PoolManager.Instance.Despawn<MagicProjectile>(this);
                return;
            }
            foreach (IEffect effect in effects)
            {
                if (effect == null) continue;
                if (towerData.EffectChance < 1.0f) effect.Apply(target, towerData, towerData.EffectChance);
                else effect.Apply(target, towerData);
            }

            
            OnDespawn();
            PoolManager.Instance.Despawn<MagicProjectile>(this);

        }
    }

}
