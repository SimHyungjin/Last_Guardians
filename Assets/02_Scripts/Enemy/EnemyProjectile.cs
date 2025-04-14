using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBase
{
    public MonsterData Data {  get; set; }

    public override void Update()
    {
        base.Update();
    }

    public override void Launch(Vector2 targetPos)
    {
        base.Launch(targetPos);
    }

    public override void ProjectileMove()
    {
        base.ProjectileMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //맵밖으로 벗어났을때도 오브젝트 풀에 반납

        if (collision.gameObject.layer == LayerMask.NameToLayer("Center"))
        {
            //데미지 호출
            PoolManager.Instance.Despawn(this);
        }
    }
}
