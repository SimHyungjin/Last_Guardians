using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMonster : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] protected MonsterSkillData skillData;
    public float CurrentHP { get; set; }
    public float SpeedModifier { get; set; } = 1f;
    public float DefModifier { get; set; } = 1f;
    protected float attackDelay = 3f;
    protected float attackTimer = 0f;
    private bool isAttack = false;
    protected float skillTimer = 0f;
    [SerializeField] private float attackRange = 0.5f;

    public LayerMask targetLayer;
    public Transform Target { get; set; }

    private SpriteRenderer spriteRenderer;
    protected NavMeshAgent agent;
    private RaycastHit2D[] hit;

    //도트데미지 관련 필드
    private float dotDuration = 0f;
    private float dotDamage = 0f;
    private Coroutine dotDamageCorutine;
    //스턴 관련 필드
    private Coroutine sturnCorutine;
    private float sturnDuration;
    private bool isSturn = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hit = new RaycastHit2D[4];
        if(skillData!=null)
            skillTimer = skillData.skillCoolTime;
    }

    private void OnEnable()
    {
        //오프젝트 풀에서 다시 꺼내졌을때 초기화
        CurrentHP = monsterData.maxHP;
        agent.isStopped = false;
        agent.speed = monsterData.speed;
        isAttack = false;
    }

    private void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.left, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.up, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.right, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.down, Color.red);

        if(isAttack)
            attackTimer -= Time.deltaTime;

        if (isAttack && attackTimer <= 0)
        {
            Attack();
        }

        if(skillData != null)
        {
            if (!isSturn)
                skillTimer -= Time.deltaTime;

            if (skillTimer <= 0)
            {
                MonsterSkill();
            }
        }
    }

    private void FixedUpdate()
    {
        //왼쪽 오른쪽 뒤집기
        if (transform.position.x > 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        if(!isAttack)
            Move();

        if(!isAttack)
        {
            //레이캐스트 쏘기
            hit[0] = Physics2D.Raycast(this.transform.position, Vector2.left, attackRange, targetLayer);
            hit[1] = Physics2D.Raycast(this.transform.position, Vector2.up, attackRange, targetLayer);
            hit[2] = Physics2D.Raycast(this.transform.position, Vector2.down, attackRange, targetLayer);
            hit[3] = Physics2D.Raycast(this.transform.position, Vector2.right, attackRange, targetLayer);
            foreach (var hit in hit)
            {
                if (hit.collider != null)
                {
                    isAttack = true;
                    break;
                }
            }
        }
        
    }

    private void Move()
    {
        agent.SetDestination(Target.position);
    }

    protected virtual void Attack()
    {
        agent.isStopped = true;
        agent.speed = 0f;
        //타입별 몬스터에서 구현
    }

    private void Death()
    {
        //사망애니메이션 재생 후 오브젝트 풀에 반납하기
        PoolManager.Instance.Despawn(this);
    }

    protected virtual void MonsterSkill()
    {
        //실구현은 상속받는곳에서
    }

    //데미지 받을 떄 호출되는 함수
    public void TakeDamage(float amount)
    {
        //데미지 관련 공식 들어가야 함
        CurrentHP -= amount;
        if(CurrentHP <= 0)
            Death();
    }

    //도트 데미지 구현
    public void DotDamage(float dotdamage, float duration)
    {
        if (dotDamageCorutine != null)
        {
            dotDuration = Mathf.Max(dotDuration, duration);
            dotDamage = Mathf.Min(dotDamage, dotdamage);
        }

        else
        {
            dotDuration = duration;
            dotDamage = dotdamage;
            dotDamageCorutine = StartCoroutine(InflictDamageOverTime());
        }
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (dotDuration < 0)
        {
            //데미지 관련 공식 나오면 수정
            TakeDamage(dotDamage);

            yield return new WaitForSeconds(1f);

            dotDuration -= 1f;
        }

        dotDamageCorutine = null;
        dotDamage = 0f;
        dotDuration = 0f;
    }

    //스턴 구현
    public void ApplySturn(float amount, float duration)
    {
        TakeDamage(amount);
        if (sturnCorutine != null)
        {
            sturnDuration = Mathf.Max(sturnDuration, duration);
        }

        else
        {
            sturnDuration = duration;
            dotDamageCorutine = StartCoroutine(SturnOverTime());
        }
    }

    private IEnumerator SturnOverTime()
    {
        isSturn = true;
        while (sturnDuration < 0)
        {
            agent.speed = 0f;

            yield return new WaitForSeconds(0.1f);

            sturnDuration -= 0.1f;
        }

        sturnCorutine = null;
        sturnDuration = 0f;
        agent.speed = monsterData.speed;
        isSturn = false;
    }

    public int GetMonsterID()
    {
        return monsterData.monsterID;
    }

}
