using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    public MonsterData monsterData;
    public float CurrentHP { get; set; }
    public float SpeedModifier { get; set; } = 1f;
    public float DefModifier { get; set; } = 1f;
    protected float skillTimer;
    public LayerMask targetLayer;
    private Vector3 localScale;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D hit;

    private void Start()
    {
        //첫 로컬스케일 저장
        localScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //현재체력 초기화
        CurrentHP = monsterData.maxHP;
    }

    private void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.left, Color.red);
    }

    private void FixedUpdate()
    {
        //왼쪽 오른쪽 뒤집기
        if (transform.position.x > 0)
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        else
            transform.localScale = new Vector3(localScale.x,localScale.y,localScale.z);

        Move();

        //레이캐스트 쏘기 사거리는 임시값
        hit = Physics2D.Raycast(this.transform.position, Vector2.left, 1, targetLayer);
        if(hit.collider != null)
        {
            Attack();
        }
    }

    private void Move()
    {

    }

    private void Attack()
    {
        //정지 후 공격 애니메이션 재생 후 플레이어의 체력을 깎기
    }

    private void Death()
    {
        //사망애니메이션 재생 후 오브젝트 풀에 반납하기
        PoolManager.Instance.Despawn(this);
    }

    protected virtual void MonsterSkill(MonsterData monsterData)
    {
        //실구현은 상속받는곳에서
    }

    private void PathFind()
    {

    }

    //데미지 받을 떄 호출되는 함수
    public void TakeDamage(float amount)
    {
        CurrentHP -= amount;
        if(CurrentHP <= 0)
            Death();
    }

}
