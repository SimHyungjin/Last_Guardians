using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BaseMonster : MonoBehaviour
{
    public MonsterData MonsterData { get; private set; }
    public MonsterSkillBase MonsterSkillBaseData { get; private set; }
    [SerializeField] private Transform prefabSlot;
    private SPUM_Prefabs currentPrefab;

    //몬스터 스탯관련
    public float CurrentHP { get; set; }
    public float DeBuffSpeedModifier { get; set; } = 1f;
    public float BuffSpeedModifier {  get; set; } = 1f;
    public float CurrentSpeed;
    public float CurrentDef { get; set; }
    public float DeBuffDefModifier { get; set; } = 1f;
    public float BuffDefModifier { get; set; } = 1f;
    public float CurrentSkillValue { get; set; } = 1f;
    public float SkillValueModifier { get; set; } = 1f;
    public float DefConstant { get; private set; } = 10f;

    //근접사거리 원거리 사거리
    private float meleeAttackRange = 0.8f;
    private float rangedAttackRange = 2.0f;

    //몬스터 공격관련
    private float AttackRange;
    protected float attackDelay = 1f;
    public float AttackTimer { get; protected set; }
    private bool isAttack = false;
    public float SkillTimer { get; protected set; }
    protected bool firstHit = false;
    public int FirstHitDamage { get; private set; }
    public int SecondHitDamage { get; private set; }
    protected int disableAttackCount; // 이 숫자만큼 몬스터가 공격하면 사라짐
    protected int attackCount = 0;
    protected bool isDisable = false;

    //목표지점 관련
    public LayerMask targetLayer;
    public Transform Target { get; set; } // 목표지점

    private AnimationConnect animationConnect;
    //private Animator animator;
    private List<SpriteRenderer> spriteRenderers = new();
    private List<Color> originalColors = new();
    private Color hitColor = Color.red; // 데미지 입었을 때 색상
    private int blinkCount = 2; // 번쩍이는 횟수
    private float blinkInterval = 0.1f; // 깜빡이는 주기
    private Coroutine colorCoroutine;
    private readonly Vector3 rightScale = new Vector3(1, 1, 1);
    private readonly Vector3 leftScale = new Vector3(-1, 1, 1);

    protected NavMeshAgent agent;

    //상태이상 핸들러 관련 필드
    private EffectHandler effectHandler;
    private StatusEffect dotDamage;
    private StatusEffect slowDown;
    private StatusEffect sturn;
    private StatusEffect defDown;
    private StatusEffect defBuff;
    private StatusEffect speedBuff;
    private StatusEffect evasionBuff;
    private StatusEffect skillValueDebuff;
    private StatusEffect silenceDebuff;
    public bool isSturn = false;
    public bool isSilence = false;
    public float EvasionRate { get; set; } = -1f;

    public Action OnMonsterDeathAction;

    private WaitForSeconds blinkSeconds;

    private Vector2 movedirection;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        effectHandler = GetComponent<EffectHandler>();
        blinkSeconds = new WaitForSeconds(blinkInterval);
    }

    public void Setup(MonsterData data, MonsterSkillBase skillData = null)
    {
        this.MonsterData = data;
        //DestroyAllChildren(prefabSlot);
        if(currentPrefab != null)
            PoolManager.Instance.DespawnbyPrefabName(currentPrefab);
        //currentPrefab = Instantiate(MonsterData.Prefab,prefabSlot);
        currentPrefab = PoolManager.Instance.SpawnbyPrefabName(MonsterData.Prefab, prefabSlot);
        currentPrefab.transform.SetParent(prefabSlot);

        if (this.transform.position.x < 0)
        {
            this.transform.localScale = rightScale;
        }
        else
        {
            this.transform.localScale = leftScale;
        }
        currentPrefab.transform.localScale = rightScale;

        if (skillData != null)
            this.MonsterSkillBaseData = skillData;
        else this.MonsterSkillBaseData = null;
        Init();
    }

    private void Init()
    {
        var existingConnect = currentPrefab.GetComponentInChildren<AnimationConnect>();
        if (existingConnect == null)
        {
            animationConnect = currentPrefab.gameObject.AddComponent<AnimationConnect>();
        }
        else
        {
            animationConnect = existingConnect;
        }
        animationConnect.Animator = null;
        animationConnect.Animator = currentPrefab.GetComponentInChildren<Animator>();
        animationConnect.BaseMonster = null;
        animationConnect.BaseMonster = this;
        
        originalColors.Clear();
        spriteRenderers.Clear();
        spriteRenderers = currentPrefab.GetComponentsInChildren<SpriteRenderer>().ToList();
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            originalColors.Add(spriteRenderers[i].color);
        }

        AttackRange = MonsterData.MonsterAttackPattern == MonAttackPattern.Ranged ? rangedAttackRange : meleeAttackRange;
        if(MonsterData.MonsterType != MonType.Standard)
            CurrentHP = MonsterData.MonsterHP * MonsterManager.Instance.nowWave.BossMultiplier;
        else
            CurrentHP = MonsterData.MonsterHP;
        CurrentDef = MonsterData.MonsterDef;
        AttackTimer = 0f;
        agent.isStopped = false;
        agent.speed = MonsterData.MonsterSpeed;
        
        isAttack = false;
        firstHit = false;
        colorCoroutine = null;
        FirstHitDamage = MonsterData.MonsterDamage;
        SecondHitDamage = 1;
        disableAttackCount = MonsterData.MonsterType == MonType.Standard ? 6 : 26;
        attackCount = 0;
        isDisable = false;
        if (MonsterData.HasSkill)
        {
            MonsterSkillBaseData = MonsterManager.Instance.MonsterSkillDatas.Find(a => a.skillData.SkillIndex == MonsterData.MonsterSkillID);
            SkillTimer = MonsterSkillBaseData.skillData.SkillCoolTime;
            Debug.Log($"{MonsterData.MonsterName} : {MonsterSkillBaseData.skillData.SkillName} 가지고 있음");
        }

        effectHandler.ClearAllEffect();

    }

    private void Update()
    {
        if (isAttack)
            AttackTimer -= Time.deltaTime;

        if (isAttack && !isSturn && AttackTimer <= 0)
        {
            StartAttack();
        }

        if (MonsterSkillBaseData != null)
        {
            if (!isSturn || !isSilence)
                SkillTimer -= Time.deltaTime;

            if (SkillTimer <= 0)
            {
                SkillTimer = MonsterSkillBaseData.skillData.SkillCoolTime;
                if (Random.Range(0f, 1f) <= MonsterSkillBaseData.skillData.MonsterskillProbablilty)
                    MonsterSkill();
            }
        }

        ApplyStatus();

        if (agent.speed == 0)
        {
            animationConnect.StopMoveAnimation();
        }
    }

    private void ApplyStatus()
    {
        CurrentSpeed = MonsterData.MonsterSpeed * BuffSpeedModifier * DeBuffSpeedModifier;
        agent.speed = CurrentSpeed;
        CurrentDef = MonsterData.MonsterDef * BuffDefModifier * DeBuffDefModifier;
        if (MonsterData.HasSkill)
        {
            CurrentSkillValue = MonsterSkillBaseData.skillData.MonsterskillEffectValue * SkillValueModifier;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, AttackRange);
    }

    private void FixedUpdate()
    {
        //왼쪽 오른쪽 뒤집기
        FlipMonsterbyDirection();

        if (!isAttack && !isSturn)
            Move();

        if(!isAttack && Physics2D.OverlapCircle(this.transform.position,AttackRange,targetLayer))
        {
            isAttack = true;
            agent.SetDestination(transform.position);
        }
    }

    private void FlipMonsterbyDirection()
    {
        movedirection = new Vector2(agent.velocity.x,agent.velocity.y).normalized;

        if (movedirection.x > 0)
        {
            this.transform.localScale = leftScale;
        }
        else if(movedirection.x < 0)
        {
            this.transform.localScale = rightScale;
        }
    }   

    private void Move()
    {
        animationConnect.StartMoveAnimation();
        agent.SetDestination(Target.position);
    }

    private void StartAttack()
    {
        animationConnect.StartAttackAnimation();
        agent.isStopped = true;
        agent.speed = 0f;   
    }

    public void Attack()
    {
        if (MonsterData.MonsterAttackPattern == MonAttackPattern.Ranged)
            RangeAttack();
        else
            MeleeAttack();
    }

    public virtual void MeleeAttack()
    {
        //타입별 몬스터에서 구현
    }

    public virtual void RangeAttack()
    {
        
        //타입별 몬스터에서 구현

    }

    protected void AfterAttack()
    {
        attackCount++;
        if (attackCount >= disableAttackCount)
        {
            isDisable = true;
            Debug.Log("횟수 다 되서 죽음");
            Death();
        }
    }

    public virtual void Death()
    {
        //사망애니메이션 재생 후 오브젝트 풀에 반납하기 오브젝트 풀 반납은 상속받은 스크립트에서
        for (int i = 0; i < spriteRenderers.Count; i++)
            spriteRenderers[i].color = originalColors[i];
        MonsterManager.Instance.OnMonsterDeath(this);
        OnMonsterDeathAction?.Invoke();
        if (!isDisable)
        {
            animationConnect.StartDeathAnimaiton();
            MonsterManager.Instance.MonsterKillCount++;
            EXPBead bead = PoolManager.Instance.Spawn<EXPBead>(MonsterManager.Instance.EXPBeadPrefab,InGameManager.Instance.transform);
            bead.Init(MonsterData.Exp, this.transform);
            PoolManager.Instance.Despawn<SPUM_Prefabs>(currentPrefab);
        }
    }
    protected virtual void MonsterSkill()
    {
        //실구현은 상속받는곳에서
    }

    //데미지 받을 떄 호출되는 함수
    public virtual void TakeDamage(float amount, float penetration = 0)
    {
        Debug.Log($"데미지 입음{amount}");
        if(EvasionRate != -1f)
        {
            if (Random.Range(0f, 1f) * 100 < EvasionRate)
            {
                return;
            }
        }

        //데미지 관련 공식 들어가야 함
        //CurrentHP -= amount;
        CurrentHP -= amount * (1 - CurrentDef * (1-penetration)/ (CurrentDef * (1 - penetration) + DefConstant));

        if (CurrentHP <= 0)
            animationConnect.StartDeathAnimaiton();
        
        //피격시 몬스터 색 변경
        if (this.gameObject.activeSelf)
        {
            if (colorCoroutine != null)
            {
                StopCoroutine(BlinkCoroutine());
            }
            colorCoroutine = StartCoroutine(BlinkCoroutine());
        }
    }

    private IEnumerator BlinkCoroutine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            for (int j = 0; j < spriteRenderers.Count; j++)
            {
                spriteRenderers[j].color = hitColor;
            }
            //spriteRenderers.color = hitColor;
            yield return blinkSeconds;
            for (int j = 0; j < spriteRenderers.Count; j++)
            {
                spriteRenderers[j].color = originalColors[j];
            }
            //spriteRenderers.color = originalColors;
            yield return blinkSeconds;
        }

       
        colorCoroutine = null;
    }
    //도트 데미지 적용
    public void DotDamage(float amount, float duration)
    {
        if(effectHandler.IsInEffect(dotDamage))
        {
            dotDamage = new DotDamageEffect(amount, duration);
            effectHandler.AddEffect(dotDamage);
        }
        else
        {
            dotDamage.UpdateEffectTime(amount, duration);
        }
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
        if (effectHandler.IsInEffect(sturn))
        {
            sturn = new SturnEffect(amount, duration);
            effectHandler.AddEffect(sturn);
        }
        else
        {
            sturn.UpdateEffectTime(amount, duration);
        }
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
        if (effectHandler.IsInEffect(slowDown))
        {
            slowDown = new SlowEffect(amount, duration);
            effectHandler.AddEffect(slowDown);
        }
        else
        {
            slowDown.UpdateEffectTime(amount, duration);
        }
    }

    //슬로우 디버프 해제
    public void CancelSlowdown()
    {
        effectHandler.RemoveEffect(slowDown);
    }

    //방어력 감소 적용
    public void ApplyReducionDef(float amount, float duration)
    {
        if (effectHandler.IsInEffect(defDown))
        {
            defDown = new DefDownEffect(amount, duration);
            effectHandler.AddEffect(defDown);
        }
        else
        {
            defDown.UpdateEffectTime(amount, duration);
        }
    }

    //방어력 디버프 해제
    public void CancelDefDown()
    {
        effectHandler.RemoveEffect(defDown);
    }

    //방어력 버프
    public void ApplyDefBuff(float amount, float duration)
    {
        if (effectHandler.IsInEffect(defBuff))
        {
            defBuff = new DefBuffEffect(amount, duration);
            effectHandler.AddEffect(defBuff);
        }
        else
        {
            defBuff.UpdateEffectTime(amount, duration);
        }
    }

    //방어력 버프 해제
    public void CancelDefBuff()
    {
        effectHandler.RemoveEffect(defBuff);
    }

    //이동속도 버프
    public void ApplySpeedBuff(float amount, float duration)
    {
        if (effectHandler.IsInEffect(speedBuff))
        {
            speedBuff = new SpeedBuffEffect(amount, duration);
            effectHandler.AddEffect(speedBuff);
        }
        else
        {
            speedBuff.UpdateEffectTime(amount, duration);
        }
    }

    //이속 버프 해제
    public void CancelSpeedBuff()
    {
        effectHandler.RemoveEffect(speedBuff);
    }

    //회피율 버프
    public void ApplyEvasionBuff(float amount, float duration)
    {
        if (effectHandler.IsInEffect(evasionBuff))
        {
            evasionBuff = new EvasionBuffEffect(amount, duration);
            effectHandler.AddEffect(evasionBuff);
        }
        else
        {
            evasionBuff.UpdateEffectTime(amount,duration);
        }
    }

    //회피 버프 해제
    public void CancelEvasionBuff()
    {
        effectHandler.RemoveEffect(evasionBuff);
    }

    public void ApplySkillValueDebuff(float amount, float duration)
    {
        if (effectHandler.IsInEffect(skillValueDebuff))
        {
            skillValueDebuff = new SkillValueDebuffEffect(amount, duration);
            effectHandler.AddEffect(skillValueDebuff);
        }
        else
        {
            skillValueDebuff.UpdateEffectTime(amount,duration);
        }
    }

    public void CancelSkillValueDebuff()
    {
        effectHandler.RemoveEffect(skillValueDebuff);
    }

    public void ApplySilenceDebuff(float duration)
    {
        if (effectHandler.IsInEffect(silenceDebuff))
        {
            silenceDebuff = new SilenceDebuff(0, duration);
            effectHandler.AddEffect(silenceDebuff);
        }
        else
        {
            silenceDebuff.UpdateEffectTime(0, duration);
        }
    }

    public void CancelSilenceDebuff()
    {
        effectHandler.RemoveEffect(silenceDebuff);
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

    //넉백 적용
    public void ApplyKnockBack(float distance, float speed, Vector2 attackerPosition)
    {
        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * distance;

        transform.DOMove(targetPosition, speed).SetEase(Ease.OutQuad);
    }

    public bool IsFirstHit()
    {
        return firstHit;
    }

    public void SetFirstHit()
    {
        firstHit = true;
    }

}
