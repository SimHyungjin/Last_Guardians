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
                if (Data.MonsterType == MonType.Standard)
                {
                    InGameManager.Instance.TakeDmage(1);
                }
                else if (Data.MonsterType == MonType.Boss)
                {
                    InGameManager.Instance.TakeDmage(5);
                }
            }
            else
            {
                InGameManager.Instance.TakeDmage(2);
            }
            
            PoolManager.Instance.Despawn(this);
        }
    }
}
