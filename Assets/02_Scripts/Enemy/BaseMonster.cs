using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BaseMonster : MonoBehaviour
{
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected MonsterSkillData skillData;

    //몬스터 스탯관련
    public float CurrentHP { get; set; }
    public float SpeedModifier { get; set; } = 1f;
    public float CurrentDef { get; set; }
    public float DefModifier { get; set; } = 1f;


    //몬스터 공격관련
    private float meleeAttackRange;
    protected float attackDelay = 3f;
    protected float attackTimer = 0f;
    private bool isAttack = false;
    protected float skillTimer = 0f;

    //목표지점 관련
    public LayerMask targetLayer;
    public Transform Target { get; set; } // 목표지점

    private SpriteRenderer spriteRenderer;
    protected NavMeshAgent agent;
    private RaycastHit2D[] hit;

    //코루틴
    private WaitForSeconds zeropointone;
    private WaitForSeconds onesec;

    //상태이상관련
    //도트데미지 관련 필드
    private float dotDuration = 0f;
    private float dotDamage = 0f;
    private Coroutine dotDamageCorutine;
    //스턴 관련 필드
    private Coroutine sturnCorutine;
    private float sturnDuration;
    private bool isSturn = false;
    //이속저하 필드
    private float slowDownDuration;
    private Coroutine slowDownCorutine;
    //방어력감소 필드
    private float reducDefDuration;
    private Coroutine reduceDefCorutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        hit = new RaycastHit2D[4];
        zeropointone = new WaitForSeconds(0.1f);
        onesec = new WaitForSeconds(1f);
    }

    public void Setup(MonsterData data, MonsterSkillData skillData = null)
    {
        this.monsterData = data;
        if (skillData != null)
            this.skillData = skillData;
        else this.skillData = null;
        init();
    }

    private void init()
    {
        meleeAttackRange = monsterData.MonsterAttackPattern == MonAttackPattern.Ranged ? 2f : 0.5f;
        spriteRenderer.sprite = monsterData.Image;
        CurrentHP = monsterData.MonsterHP;
        CurrentDef = monsterData.MonsterDef;
        agent.isStopped = false;
        agent.speed = monsterData.MonsterSpeed;
        isAttack = false;
        dotDuration = 0f;
        sturnDuration = 0f;
        attackTimer = 0f;
        if (monsterData.HasSkill)
        {
            skillData = MonsterManager.Instance.MonsterSkillDatas.Find(a => a.SkillIndex == monsterData.MonsterSkillID);
            skillTimer = skillData.SkillCoolTime;
        }   
    }

    private void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.left, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.up, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.right, Color.red);
        Debug.DrawRay(this.transform.position, Vector2.down, Color.red);

        if(isAttack)
            attackTimer -= Time.deltaTime;

        if (isAttack && !isSturn && attackTimer <= 0)
        {
            Attack();
        }

        if(skillData != null)
        {
            if (!isSturn)
                skillTimer -= Time.deltaTime;

            if (skillTimer <= 0)
            {
                skillTimer = skillData.SkillCoolTime;
                if (Random.Range(0f, 1f) <= skillData.MonsterskillProbablilty)
                    MonsterSkill();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, meleeAttackRange);
    }

    private void FixedUpdate()
    {
        //왼쪽 오른쪽 뒤집기
        if (transform.position.x > 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        if(!isAttack && !isSturn)
            Move();

        if(!isAttack && Physics2D.OverlapCircle(this.transform.position,meleeAttackRange,targetLayer))
        {
            isAttack = true;
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

    protected virtual void Death()
    {
        //사망애니메이션 재생 후 오브젝트 풀에 반납하기 오브젝트 풀 반납은 상속받은 스크립트에서
        StopAllCoroutines();
        sturnCorutine = null;
        dotDamageCorutine = null;
        reduceDefCorutine = null;
        slowDownCorutine = null;
        MonsterManager.Instance.OnMonsterDeath();
    }

    protected virtual void MonsterSkill()
    {
        //실구현은 상속받는곳에서
    }

    public int GetMonsterID()
    {
        return monsterData.MonsterIndex;
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
            if (gameObject.activeSelf)
                dotDamageCorutine = StartCoroutine(InflictDamageOverTime());
        }
    }

    private IEnumerator InflictDamageOverTime()
    {
        while (dotDuration > 0)
        {
            //데미지 관련 공식 나오면 수정
            TakeDamage(dotDamage);
            Debug.Log($"도트데미지 적용 {dotDamage} 남은체력 : {CurrentHP} 남은 시간 : {dotDuration}");
            yield return onesec;

            dotDuration -= 1f;
        }

        dotDamageCorutine = null;
        dotDamage = 0f;
        dotDuration = 0f;
    }

    //스턴 구현
    public void ApplySturn(float duration, float amount = 0)
    {
        TakeDamage(amount);
        if (sturnCorutine != null)
        {
            sturnDuration = Mathf.Max(sturnDuration, duration);
        }
        else
        {
            sturnDuration = duration;
            if(gameObject.activeSelf)
                sturnCorutine = StartCoroutine(SturnOverTime());
        }
    }

    private IEnumerator SturnOverTime()
    {
        isSturn = true;
        agent.SetDestination(this.transform.position);
        Debug.Log($"스턴적용");
        while (sturnDuration > 0)
        {
            yield return zeropointone;

            sturnDuration -= 0.1f;
        }
        sturnCorutine = null;
        sturnDuration = 0f;
        isSturn = false;
    }

    //슬로우 적용
    public void ApplySlowdown(float duration, float amount)
    {
        if(slowDownCorutine != null)
        {
            slowDownDuration = Mathf.Max(slowDownDuration, duration);
            SpeedModifier = Mathf.Max(SpeedModifier, amount);
        }
        else
        {
            slowDownDuration = duration;
            SpeedModifier = amount;
            if (gameObject.activeSelf)
                slowDownCorutine = StartCoroutine(SlowDownOver(SpeedModifier));
        }
    }

    private IEnumerator SlowDownOver(float amount)
    {
        agent.speed = agent.speed * (1 - amount);
        while (slowDownDuration > 0)
        {
            Debug.Log($"슬로우적용 현재 이속 : {agent.speed} 남은시간 : {sturnDuration}");
            yield return zeropointone;

            slowDownDuration -= 0.1f;
        }
        agent.speed = monsterData.MonsterSpeed;
        slowDownCorutine = null;
        slowDownDuration = 0f;
    }

    //방어력 감소 적용
    public void ApplyReducionDef(float duration, float amount)
    {
        if(reduceDefCorutine != null)
        {
            reducDefDuration = Mathf.Max(reducDefDuration, duration);
            DefModifier = Mathf.Max(DefModifier, amount);
        }
        else
        {
            duration = reducDefDuration;
            DefModifier = amount;
            if (gameObject.activeSelf)
                reduceDefCorutine = StartCoroutine(DefDownOver(DefModifier));
        }
    }

    private IEnumerator DefDownOver(float amount)
    {
        CurrentDef = CurrentDef * (1 - amount);
        while (reducDefDuration > 0)
        {
            Debug.Log($"방깎적용 현재 방어력 : {CurrentDef} 남은시간 : {reducDefDuration}");
            yield return zeropointone;

            reducDefDuration -= 0.1f;
        }

        CurrentDef = monsterData.MonsterDef;
        reduceDefCorutine = null;
        reducDefDuration = 0f;
    }
}
