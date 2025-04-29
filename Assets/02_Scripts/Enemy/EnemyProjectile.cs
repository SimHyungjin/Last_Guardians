using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBase
{
    public MonsterData Data {  get; set; }
    public BaseMonster BaseMonster { get; set; }

    public override void Update()
    {
        base.Update();
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
