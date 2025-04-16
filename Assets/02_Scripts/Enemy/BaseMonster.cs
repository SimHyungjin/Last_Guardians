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
    [SerializeField] protected MonsterSkillBase monsterSkill;

    //몬스터 스탯관련
    public float CurrentHP { get; set; }
    public float DeBuffSpeedModifier { get; set; } = 1f;
    public float BuffSpeedModifier {  get; set; } = 1f;
    public float CurrentDef { get; set; }
    public float DeBuffDefModifier { get; set; } = 1f;
    public float BuffDefModifier { get; set; } = 1f;

    //근접사거리 원거리 사거리
    [SerializeField] private float meleeAttackRange = 0.5f;
    [SerializeField] private float RangedAttackRange = 2.0f;

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

    //상태이상 핸들러 관련 필드
    private EffectHandler effectHandler;
    private StatusEffect dotDamage;
    private StatusEffect slowDown;
    private StatusEffect sturn;
    private StatusEffect defDown;
    private StatusEffect defBuff;
    private StatusEffect speedBuff;
    private StatusEffect EvasionBuff;
    public bool isSturn = false;
    public float EvasionRate { get; set; } = -1f;

    public Action OnMonsterDeathAction;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        effectHandler = GetComponent<EffectHandler>();
    }

    public void Setup(MonsterData data, MonsterSkillBase skillData = null)
    {
        this.monsterData = data;
        if (skillData != null)
            this.monsterSkill = skillData;
        else this.monsterSkill = null;
        Init();
    }

    private void Init()
    {
        AttackRange = monsterData.MonsterAttackPattern == MonAttackPattern.Ranged ? RangedAttackRange : meleeAttackRange;
        CancelAllBuff();
        CancelAllDebuff();
        spriteRenderer.sprite = monsterData.Image;
        CurrentHP = monsterData.MonsterHP;
        CurrentDef = monsterData.MonsterDef;
        attackTimer = 0f;
        agent.isStopped = false;
        agent.speed = monsterData.MonsterSpeed;
        isAttack = false;
        if (monsterData.HasSkill)
        {
            monsterSkill = MonsterManager.Instance.MonsterSkillDatas.Find(a => a.skillData.SkillIndex == monsterData.MonsterSkillID);
            skillTimer = monsterSkill.skillData.SkillCoolTime;
        }   
    }


    private void Update()
    {
        if(isAttack)
            attackTimer -= Time.deltaTime;

        if (isAttack && !isSturn && attackTimer <= 0)
        {
            if (monsterData.MonsterAttackPattern == MonAttackPattern.Ranged)
                RangeAttack();
            else
                MeleeAttack();
        }

        if(monsterSkill != null)
        {
            if (!isSturn)
                skillTimer -= Time.deltaTime;

            if (skillTimer <= 0)
            {
                skillTimer = monsterSkill.skillData.SkillCoolTime;
                if (Random.Range(0f, 1f) <= monsterSkill.skillData.MonsterskillProbablilty)
                    MonsterSkill();
            }
        }

        ApplyStatus();
    }

    private void ApplyStatus()
    {
        agent.speed = monsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
        CurrentDef = monsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
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
        MonsterManager.Instance.OnMonsterDeath();
        OnMonsterDeathAction?.Invoke();
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

    //도트 데미지 적용
    public void DotDamage(float amount, float duration)
    {
        dotDamage = new DotDamageEffect(amount, duration);
        effectHandler.AddEffect(dotDamage);
    }

    //도트 데미지 해제
    public void CancelDotDamage()
    {
        effectHandler.RemoveEffect(dotDamage);
    }

    //스턴 구현
    public void ApplySturn(float duration, float amount = 0)
    {
        TakeDamage(amount);
        sturn = new SturnEffect(amount, duration);
        effectHandler.AddEffect(sturn);
    }

    //스턴 해제
    public void CancelSturn()
    {
        effectHandler.RemoveEffect(sturn);
    }

    public void SetDestination(Transform target)
    {
        agent.SetDestination(target.position);
    }

    //슬로우 적용
    public void ApplySlowdown(float amount, float duration)
    {
        slowDown = new SlowEffect(amount, duration);
        effectHandler.AddEffect(slowDown);
    }

    //슬로우 디버프 해제
    public void CancelSlowdown()
    {
        effectHandler.RemoveEffect(slowDown);
    }

    //방어력 감소 적용
    public void ApplyReducionDef(float amount, float duration)
    {
        defDown = new DefDownEffect(amount, duration);
        effectHandler.AddEffect(defDown);
    }

    //방어력 디버프 해제
    public void CancelDefDown()
    {
        effectHandler.RemoveEffect(defDown);
    }

    //넉백 적용
    public void ApplyKnockBack(float distance, float speed, Vector2 attackerPosition)
    {
        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * distance;

        transform.DOMove(targetPosition, speed).SetEase(Ease.OutQuad);
        
    }

    //방어력 버프
    public void ApplyDefBuff(float amount, float duration)
    {
        defBuff = new DefBuffEffect(amount, duration);
        effectHandler.AddEffect(defBuff);
    }

    //방어력 버프 해제
    public void CancelDefBuff()
    {
        effectHandler.RemoveEffect(defBuff);
    }

    //이동속도 버프
    public void ApplySpeedBuff(float amount, float duration)
    {
        speedBuff = new SpeedBuffEffect(amount, duration);
        effectHandler.AddEffect(speedBuff);
    }


    //이속 버프 해제
    public void CancelSpeedBuff()
    {
        effectHandler.RemoveEffect(speedBuff);
    }

    //회피율 버프
    public void ApplyEvasionBuff(float amount, float duration)
    {
        EvasionBuff = new EvasionBuffEffect(amount, duration);
        effectHandler.AddEffect(EvasionBuff);
    }

    //회피 버프 해제
    public void CancelEvasionBuff()
    {
        effectHandler.RemoveEffect(EvasionBuff);
    }

    //전체 디버프 해제
    public void CancelAllDebuff()
    {
        effectHandler.RemoveAllDeBuff();
    }

    //전체 버프 해제
    public void CancelAllBuff()
    {
        effectHandler.RemoveAllBuff();
    }
}
