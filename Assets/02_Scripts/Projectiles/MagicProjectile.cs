using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicProjectile : ProjectileBase
{
    public BaseMonster target;
    private bool hasHit = false;
    public override void Init(TowerData _towerData)
    {
        base.Init(_towerData);
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/{spritename}.png";
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
        effect = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            hasHit = true;
            BaseMonster target = collision.GetComponent<BaseMonster>();
            if (target == null) return;
            target.TakeDamage(towerData.AttackPower);
            if (towerData.SpecialEffect == SpecialEffect.None || effect == null) return;
            if (towerData.EffectChance < 1.0f) effect.Apply(target, towerData, towerData.EffectChance);
            else effect.Apply(target, towerData);
            OnDespawn();
            PoolManager.Instance.Despawn(this);
        }
    }

}
