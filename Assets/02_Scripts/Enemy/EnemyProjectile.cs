using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBase
{
    public MonsterData Data { get; set; }
    public BaseMonster BaseMonster { get; set; }

    private SpriteRenderer spriteRenderer;


    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //스프라이트 가져오기
        spriteRenderer.sprite = projectileAtlas.GetSprite("DarkArrow");
    }

    public override void Update()
    {
        base.Update();
        //spriteRenderer.
        FlipProjectilebyDirection();
    }

    public override void Init(TowerData _towerData, AdaptedAttackTowerData _adaptedTower, List<int> _effectslist, EnvironmentEffect _environmentEffect)
    {
        //안쓰는 함수
    }

    //왼쪽 오른쪽 뒤집기
    private void FlipProjectilebyDirection()
    {
        Vector2 movedirection = new Vector2(rb.velocity.x, rb.velocity.y).normalized;

        if (movedirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movedirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }


    protected override void ProjectileMove()
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //맵밖으로 벗어났을때도 오브젝트 풀에 반납

        if (collision.gameObject.layer == LayerMask.NameToLayer("Center"))
        {
            //데미지 호출
            if (!BaseMonster.IsFirstHit())
            {
                BaseMonster.SetFirstHit();
                InGameManager.Instance.TakeDmage(BaseMonster.FirstHitDamage);
            }
            else
            {
                InGameManager.Instance.TakeDmage(BaseMonster.SecondHitDamage);
            }

            PoolManager.Instance.Despawn(this);
        }
    }

    public void ProjectileDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }
}
