using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BaseMonster : MonoBehaviour
{
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected MonsterSkillData skillData;

    //몬스터 스탯관련
    public float CurrentHP { get; set; }
    public float DeBuffSpeedModifier { get; set; } = 1f;
    public float BuffSpeedModifier {  get; set; } = 1f;
    public float CurrentDef { get; set; }
    public float DeBuffDefModifier { get; set; } = 1f;
    public float BuffDefModifier { get; set; } = 1f;


    //몬스터 공격관련
    private float AttackRange;
    protected float attackDelay = 3f;
    protected float attackTimer = 0f;
    private bool isAttack = false;
    protected float skillTimer = 0f;

    //목표지점 관련
    public LayerMask targetLayer;
    public Transform Target { get; set; } // 목표지점

    private SpriteRenderer spriteRenderer;
    protected NavMeshAgent agent;

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
    //버프관련
    //방어력증가 필드
    private float buffDefDuration;
    private Coroutine buffDefCorutine;
    //이동속도 증가 필드
    private float buffSpeedDuration;
    private Coroutine buffSpeedCorutine;
    //회피버프관련 필드
    public float EvasionRate { get; set; } = -1f;
    private float buffEvasionDuration;
    private Coroutine buffEvasionCorutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        AttackRange = monsterData.MonsterAttackPattern == MonAttackPattern.Ranged ? 2f : 0.5f;
        spriteRenderer.sprite = monsterData.Image;
        CurrentHP = monsterData.MonsterHP;
        CurrentDef = monsterData.MonsterDef;
        BuffDefModifier = 1f;
        BuffSpeedModifier = 1f;
        DeBuffSpeedModifier = 1f;
        DeBuffDefModifier = 1f;
        EvasionRate = 1f;
        buffDefDuration = 0f;
        buffSpeedDuration = 0f;
        reducDefDuration = 0f;
        slowDownDuration = 0f;
        dotDuration = 0f;
        buffEvasionDuration = 0f;
        sturnDuration = 0f;
        attackTimer = 0f;
        agent.isStopped = false;
        agent.speed = monsterData.MonsterSpeed;
        isAttack = false;
        isSturn = false;
        
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
            if (monsterData.MonsterAttackPattern == MonAttackPattern.Ranged)
                RangeAttack();
            else
                MeleeAttack();
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
        Gizmos.DrawWireSphere(this.transform.position, AttackRange);
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

        if(!isAttack && Physics2D.OverlapCircle(this.transform.position,AttackRange,targetLayer))
        {
            isAttack = true;
        }
        
    }

    private void Move()
    {
        agent.SetDestination(Target.position);
    }

    protected virtual void MeleeAttack()
    {
        agent.isStopped = true;
        agent.speed = 0f;
        //타입별 몬스터에서 구현
    }

    protected virtual void RangeAttack()
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
        if(EvasionRate != -1f)
        {
            if (Random.Range(0f, 1f) * 100 < EvasionRate)
            {
                return;
            }
        }

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

    //도트데미지 해제
    public void CancelInflictDamage()
    {
        if (dotDamageCorutine != null)
        {
            StopCoroutine(dotDamageCorutine);
            dotDamageCorutine = null;
            dotDamage = 0f;
            Debug.Log($"도트데미지 해제");
        }
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

    //스턴 해제
    public void CancelSturn()
    {
        if (sturnCorutine != null)
        {
            StopCoroutine(sturnCorutine);
            sturnCorutine = null;
            sturnDuration = 0f;
            isSturn = false;
            Debug.Log($"스턴 해제");
        }
    }

    //슬로우 적용
    public void ApplySlowdown(float duration, float amount)
    {
        if(slowDownCorutine != null)
        {
            slowDownDuration = Mathf.Max(slowDownDuration, duration);
            DeBuffSpeedModifier = Mathf.Max(DeBuffSpeedModifier, amount);
        }
        else
        {
            slowDownDuration = duration;
            DeBuffSpeedModifier = amount;
            if (gameObject.activeSelf)
                slowDownCorutine = StartCoroutine(SlowDownOver());
        }
    }

    private IEnumerator SlowDownOver()
    {
        
        while (slowDownDuration > 0)
        {
            agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
            Debug.Log($"슬로우적용 현재 이속 : {agent.speed}");
            yield return zeropointone;

            slowDownDuration -= 0.1f;
        }
        DeBuffSpeedModifier = 1f;
        agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
        slowDownCorutine = null;
        slowDownDuration = 0f;
    }

    //슬로우 디버프 해제
    public void CancelSlowdown()
    {
        if (slowDownCorutine != null)
        {
            StopCoroutine(slowDownCorutine);
            slowDownCorutine = null;
            slowDownDuration = 0f;
            DeBuffSpeedModifier = 1f;
            agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
            Debug.Log($"슬로우 해제. 현재 이속 : {agent.speed} ");
        }
    }

    //방어력 감소 적용
    public void ApplyReducionDef(float duration, float amount)
    {
        if(reduceDefCorutine != null)
        {
            reducDefDuration = Mathf.Max(reducDefDuration, duration);
            DeBuffDefModifier = Mathf.Max(DeBuffDefModifier, amount);
        }
        else
        {
            reducDefDuration = duration;
            DeBuffDefModifier = amount;
            if (gameObject.activeSelf)
                reduceDefCorutine = StartCoroutine(DefDownOver());
        }
    }

    private IEnumerator DefDownOver()
    {
        while (reducDefDuration > 0)
        {
            CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
            Debug.Log($"방깎적용 현재 방어력 : {CurrentDef} ");
            yield return zeropointone;

            reducDefDuration -= 0.1f;
        }

        DeBuffDefModifier = 1f;
        CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
        reduceDefCorutine = null;
        reducDefDuration = 0f;
    }

    //방어력 디버프 해제
    public void CancelDefDown()
    {
        if (reduceDefCorutine != null)
        {
            StopCoroutine(reduceDefCorutine);
            reduceDefCorutine = null;
            reducDefDuration = 0f;
            DeBuffDefModifier = 1f;
            CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
            Debug.Log($"방깍 디버프 해제 현재방어력 : {CurrentDef} ");
        }
    }

    //넉백 적용
    public void ApplyKnockBack(float distance, float speed, Vector2 attackerPosition)
    {
        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * distance;

        transform.DOMove(targetPosition, speed).SetEase(Ease.OutQuad);
    }

    //방어력 버프
    public void ApplyDefBuff(float duration, float amount)
    {
        if(buffDefCorutine != null)
        {
            buffDefDuration = Mathf.Max(buffDefDuration, duration);
            BuffDefModifier = Mathf.Max(BuffDefModifier, amount);
        }
        else
        {
            buffDefDuration = duration;
            BuffDefModifier = amount;
            if (gameObject.activeSelf)
                buffDefCorutine = StartCoroutine(DefBuffOver());
        }
    }

    private IEnumerator DefBuffOver()
    {
        while(buffDefDuration > 0)
        {
            CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
            Debug.Log($"방어버프적용 현재 방어력 : {CurrentDef} ");
            yield return zeropointone;

            buffDefDuration -= 0.1f;
        }

        BuffDefModifier = 1f;
        CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
        buffDefCorutine = null;
        buffDefDuration = 0f;
    }

    //방어력 버프 해제
    public void CancelDefBuff()
    {
        if (buffDefCorutine != null)
        {
            StopCoroutine(buffDefCorutine);
            buffDefCorutine = null;
            buffDefDuration = 0f;
            BuffDefModifier = 1f;
            CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
            Debug.Log($"방어 버프 해제 현재방어력 : {CurrentDef} ");
        }
    }

    //이동속도 버프
    public void ApplySpeedBuff(float duration, float amount)
    {

        if (buffSpeedCorutine != null)
        {
            buffSpeedDuration = Mathf.Max(buffSpeedDuration, duration);
            BuffSpeedModifier = Mathf.Max(BuffSpeedModifier, amount);
        }
        else
        {
            buffSpeedDuration = duration;
            BuffSpeedModifier = amount;
            if (gameObject.activeSelf)
                buffSpeedCorutine = StartCoroutine(BuffSpeedOver());
        }
    }

    private IEnumerator BuffSpeedOver()
    {
        while (buffSpeedDuration > 0)
        {
            agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
            Debug.Log($"스피드버프적용 현재 이속 : {agent.speed}");
            yield return zeropointone;

            slowDownDuration -= 0.1f;
        }
        BuffSpeedModifier = 1f;
        agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
        buffSpeedCorutine = null;
        buffSpeedDuration = 0f;
    }

    //이속 버프 해제
    public void CancelSpeedBuff()
    {
        if (buffSpeedCorutine != null)
        {
            StopCoroutine(buffSpeedCorutine);
            buffSpeedCorutine = null;
            buffSpeedDuration = 0f;
            BuffSpeedModifier = 1f;
            agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
            Debug.Log($"이속 버프 해제 현재이동속도 : {agent.speed} ");
        }
    }

    //회피율 버프
    public void ApplyEvasionBuff(float duration, float amount)
    {
        if(buffEvasionCorutine != null)
        {
            buffEvasionDuration = Mathf.Max(buffEvasionDuration, duration);
            EvasionRate = Mathf.Max(EvasionRate, amount);
        }
        else
        {
            buffEvasionDuration = duration;
            EvasionRate = amount;
            buffEvasionCorutine = StartCoroutine(EvasionRateOver());
        }
    }

    private IEnumerator EvasionRateOver()
    {
        while (buffEvasionDuration > 0)
        {
            yield return zeropointone;

            buffEvasionDuration -= 0.1f;
        }

        EvasionRate = -1f;
        buffEvasionCorutine = null;
        buffEvasionDuration = 0f;
    }

    //회피 버프 해제
    public void CancelEvasionBuff()
    {
        if (buffEvasionCorutine != null)
        {
            StopCoroutine(buffEvasionCorutine);
            buffEvasionCorutine = null;
            buffEvasionDuration = 0f;
            EvasionRate = -1f;
            Debug.Log($"회피 버프 해제");
        }
    }

    public void CancelAllDebuff()
    {
        CancelDefDown();
        CancelSlowdown();
        CancelInflictDamage();
        CancelSturn();
        Debug.Log("모든 디버프 해제");
    }
}
