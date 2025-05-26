using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BaseMonster : MonoBehaviour
{
    //몬스터 정보
    public MonsterData MonsterData { get; private set; }
    public MonsterSkillBase MonsterSkillBaseData { get; private set; }


    //몬스터 스탯관련
    public float ResultHP { get; private set; }
    public float CurrentHP { get; set; }
    public float DeBuffSpeedModifier { get; set; } = 0f;
    public float BuffSpeedModifier { get; set; } = 0f;
    public float CurrentSpeed { get; set; } = 1f;
    public float ResultDef { get; private set; }
    public float CurrentDef { get; set; } = 1f;
    public float DeBuffDefModifier { get; set; } = 0f;
    public float BuffDefModifier { get; set; } = 0f;
    public float CurrentSkillValue { get; set; } = 1f;
    public float SkillValueModifier { get; set; } = 1f;
    public float DefConstant { get; private set; } = 10f;
    public float MonsterStatWeight { get; private set; } = 1f;

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
    private bool isDead = false;

    //목표지점 관련
    public LayerMask targetLayer;
    public Transform Target { get; set; } // 목표지점

    // 몬스터 외형
    [SerializeField] private Transform prefabSlot;
    private SPUM_Prefabs currentPrefab;
    public AnimationConnect AnimationConnect { get; private set; }
    private List<SpriteRenderer> spriteRenderers = new();
    private List<Color> originalColors = new();
    private Color hitColor = Color.red; // 데미지 입었을 때 색상
    private int blinkCount = 2; // 번쩍이는 횟수
    private float blinkInterval = 0.1f; // 깜빡이는 주기
    private WaitForSeconds blinkSeconds;
    private Coroutine colorCoroutine;
    private readonly Vector3 rightScale = new Vector3(1, 1, 1);
    private readonly Vector3 leftScale = new Vector3(-1, 1, 1);

    //네브메쉬
    public NavMeshAgent agent { get; protected set; }

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

    //진행방향 애니메이션 필드
    private Vector2 previousDirection = Vector2.zero;
    private float directionHoldTimer = 0f; // 몬스터 뒤집기 타이머
    private const float directionThreshold = 0.25f; // 유지 시간

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
        effectHandler = GetComponent<EffectHandler>();
        blinkSeconds = new WaitForSeconds(blinkInterval);
    }

    //몬스터 초기설정
    public void Setup(MonsterData data, MonsterSkillBase skillData = null)
    {
        this.MonsterData = data;
        //DestroyAllChildren(prefabSlot);
        if (currentPrefab != null)
            PoolManager.Instance.DespawnbyPrefabName(currentPrefab);
        //currentPrefab = Instantiate(MonsterData.Prefab,prefabSlot);
        currentPrefab = PoolManager.Instance.SpawnbyPrefabName(MonsterData.Prefab, prefabSlot);
        currentPrefab.transform.SetParent(prefabSlot);

        if (this.transform.position.x < 0)
        {
            this.transform.localScale = leftScale;
        }
        else
        {
            this.transform.localScale = rightScale;
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
            AnimationConnect = currentPrefab.gameObject.AddComponent<AnimationConnect>();
        }
        else
        {
            AnimationConnect = existingConnect;
        }
        AnimationConnect.Animator = null;
        AnimationConnect.Animator = currentPrefab.GetComponentInChildren<Animator>();
        AnimationConnect.BaseMonster = null;
        AnimationConnect.BaseMonster = this;

        originalColors.Clear();
        spriteRenderers.Clear();
        spriteRenderers = currentPrefab.GetComponentsInChildren<SpriteRenderer>().ToList();
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            originalColors.Add(spriteRenderers[i].color);
        }

        AttackRange = MonsterData.MonsterAttackPattern == MonAttackPattern.Ranged ? rangedAttackRange : meleeAttackRange;

        MonsterStatWeight = (float)(1.0 + Mathf.Max(0, (float)((MonsterManager.Instance.WaveLevel - 11) * 0.03)));

        if (MonsterData.MonsterType != MonType.Standard)
        {
            ResultHP = MonsterData.MonsterHP * MonsterManager.Instance.nowWave.BossMultiplier * MonsterStatWeight;
            CurrentHP = ResultHP;
            CurrentDef = MonsterData.MonsterDef;
            ResultDef = CurrentDef;
        }
        else
        {
            ResultHP = (float)(MonsterData.MonsterHP * (1.0 + (MonsterManager.Instance.WaveLevel * 0.06))) * MonsterStatWeight;
            CurrentHP = ResultHP;
            ResultDef = MonsterData.MonsterDef * (float)(1.0 + (MonsterManager.Instance.WaveLevel * 0.015)) * MonsterStatWeight;
            CurrentDef = ResultDef;
        }


        //CurrentDef = MonsterData.MonsterDef;
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
        isDead = false;

        if (MonsterData.HasSkill)
        {
            MonsterSkillBaseData = MonsterManager.Instance.MonsterSkillDatas.Find(a => a.skillData.SkillIndex == MonsterData.MonsterSkillID);
            SkillTimer = MonsterSkillBaseData.skillData.SkillCoolTime;
        }

        effectHandler.ClearAllEffect();

    }

    private void Update()
    {
        //공격중일때만 타이머 체크
        if (isAttack)
            AttackTimer -= Time.deltaTime;

        //공격시작 조건이 됐을때
        if (isAttack && !isSturn && AttackTimer <= 0)
        {
            StartAttack();
        }

        //스킬이 있을때
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

        if (agent.speed == 0)
        {
            AnimationConnect.StopMoveAnimation();
        }
    }

    //스탯 적용
    public void ApplyStatus()
    {
        CurrentSpeed = MonsterData.MonsterSpeed * (1 + BuffSpeedModifier) * (1 - DeBuffSpeedModifier);
        agent.speed = CurrentSpeed;
        CurrentDef = ResultDef * (1 + BuffDefModifier) * (1 - DeBuffDefModifier);
        if (MonsterData.HasSkill)
        {
            CurrentSkillValue = MonsterSkillBaseData.skillData.MonsterskillEffectValue * SkillValueModifier;
        }
    }


    private void FixedUpdate()
    {
        //왼쪽 오른쪽 뒤집기
        FlipMonsterbyDirection();

        if (!isAttack && !isSturn)
        {
            Move();
        }

        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            transform.position = agent.nextPosition;
        }

        if (!isAttack && Physics2D.OverlapCircle(this.transform.position, AttackRange, targetLayer))
        {
            isAttack = true;
            agent.SetDestination(transform.position);
        }
    }

    private void FlipMonsterbyDirection()
    {
        Vector2 currentDirection = new Vector2(agent.velocity.x, agent.velocity.y).normalized;

        // x 방향이 명확하지 않은 경우 무시
        if (Mathf.Abs(currentDirection.x) < 0.01f)
            return;

        // 이전 방향과 유사한 방향이면 타이머 증가
        if (Mathf.Sign(currentDirection.x) == Mathf.Sign(previousDirection.x))
        {
            directionHoldTimer += Time.deltaTime;
        }
        else
        {
            // 방향이 바뀌었으면 타이머 초기화
            directionHoldTimer = 0f;
            previousDirection = currentDirection;
        }

        // 방향이 일정 시간 유지되었을 때만 스케일 반전
        if (directionHoldTimer >= directionThreshold)
        {
            if (currentDirection.x > 0)
            {
                transform.localScale = leftScale;
            }
            else if (currentDirection.x < 0)
            {
                transform.localScale = rightScale;
            }

            // 갱신 이후에는 다시 초기화하여 같은 방향일 때에도 새로 측정
            directionHoldTimer = 0f;
            previousDirection = currentDirection;
        }
    }

    private void Move()
    {
        AnimationConnect.StartMoveAnimation();
        if (!agent.hasPath || agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(Target.position);
        }
    }

    //공격 시작
    private void StartAttack()
    {
        AnimationConnect.StartAttackAnimation();
        agent.isStopped = true;
        agent.speed = 0f;
    }

    //어택 애니메이션에서 호출됨
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

    //공격 후
    protected void AfterAttack()
    {
        attackCount++;
        if (attackCount >= disableAttackCount)
        {
            isDisable = true;
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
            //데미지받아서 죽은게 아니라면
            if (this is BossMonster) TowerManager.Instance.ApplyBossSlayer();
            MonsterManager.Instance.MonsterKillCount++;
            EXPBead bead = PoolManager.Instance.Spawn<EXPBead>(MonsterManager.Instance.EXPBeadPrefab, InGameManager.Instance.transform);
            bead.Init(MonsterData.Exp, this.transform);
            PoolManager.Instance.Despawn<SPUM_Prefabs>(currentPrefab);
            TowerManager.Instance.towerUpgradeData.GetTowerPoint();

            if (MonsterManager.Instance.EffectTransfer >= 1 && Random.Range(0f, 1f) <= MonsterManager.Instance.EffectTransferUpgradeValue)
                DebuffTransition();
        }
    }

    private void DebuffTransition()
    {
        foreach (var monster in Utils.OverlapCircleAllSorted(this.transform.position, 2f, LayerMaskData.monster))
        {
            if (monster.TryGetComponent<BaseMonster>(out BaseMonster baseMonster))
            {
                //if(baseMonster.effectHandler.IsInEffect())
                baseMonster.AdaptStatusEffectsInList(effectHandler.GetStatusEffects());
            }
        }
    }
    public void AdaptStatusEffectsInList(List<StatusEffect> effects)
    {
        foreach (var effect in effects)
        {
            if (!this.effectHandler.IsInEffect(effect))
            {
                effect.Duration = effect.OriginDuration;
                effect.ApplyEffect(this);
                this.effectHandler.AddEffect(effect);
            }
            else
            {
                effect.UpdateEffectTime(Mathf.Max(effect.Amount, effectHandler.GetEffect(effect).Amount), Mathf.Max(effect.Duration, effectHandler.GetEffect(effect).OriginDuration), this);
            }
        }
    }

    protected virtual void MonsterSkill()
    {
        //실구현은 상속받는곳에서
    }

    //데미지 받을 떄 호출되는 함수
    public virtual void TakeDamage(float amount, float penetration = 0, bool trueDamage = false)
    {
        if (EvasionRate != -1f)
        {
            if (Random.Range(0f, 1f) * 100 < EvasionRate * 100)
            {
                return;
            }
        }

        //데미지 관련 공식 들어가야 함
        if (trueDamage)
        {
            CurrentHP -= amount;
        }
        else
        {
            CurrentHP -= amount * (1 - CurrentDef * (1 - penetration) / (CurrentDef * (1 - penetration) + DefConstant));
            if (MonsterManager.Instance.Catalysis >= 1)
                effectHandler.AllDebuffTimerPlus(MonsterManager.Instance.CatalysisValue);
        }

        float calculatedDamage = amount * (1 - CurrentDef * (1 - penetration) / (CurrentDef * (1 - penetration) + DefConstant));
        float finalDamage = trueDamage ? amount : calculatedDamage;


        if (CurrentHP <= 0 && !isDead)
        {
            AnimationConnect.StartDeathAnimaiton();
            isDead = true;
        }

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
        if (!effectHandler.IsInEffect(dotDamage))
        {
            dotDamage = new DotDamageEffect(amount, duration);
            effectHandler.AddEffect(dotDamage);
        }
        else
        {
            dotDamage.UpdateEffectTime(amount, duration, this);
        }
    }

    //도트 데미지 해제
    public void CancelDotDamage()
    {
        effectHandler.RemoveEffect(dotDamage);
    }

    public void StopSturnAnimation()
    {
        AnimationConnect.StopSturnAnimation();
    }

    //스턴 구현
    public void ApplySturn(float duration, float amount = 0)
    {
        //TakeDamage(amount);
        if (!effectHandler.IsInEffect(sturn))
        {
            sturn = new SturnEffect(amount, duration);
            effectHandler.AddEffect(sturn);
        }
        else
        {
            sturn.UpdateEffectTime(amount, duration, this);
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
        if (!effectHandler.IsInEffect(slowDown))
        {
            slowDown = new SlowEffect(amount, duration);
            effectHandler.AddEffect(slowDown);
        }
        else
        {
            slowDown.UpdateEffectTime(amount, duration, this);
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
        if (!effectHandler.IsInEffect(defDown))
        {
            defDown = new DefDownEffect(amount, duration);
            effectHandler.AddEffect(defDown);
        }
        else
        {
            defDown.UpdateEffectTime(amount, duration, this);
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
        if (!effectHandler.IsInEffect(defBuff))
        {
            defBuff = new DefBuffEffect(amount, duration);
            effectHandler.AddEffect(defBuff);
        }
        else
        {
            defBuff.UpdateEffectTime(amount, duration, this);
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
        if (!effectHandler.IsInEffect(speedBuff))
        {
            speedBuff = new SpeedBuffEffect(amount, duration);
            effectHandler.AddEffect(speedBuff);
        }
        else
        {
            speedBuff.UpdateEffectTime(amount, duration, this);
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
        if (!effectHandler.IsInEffect(evasionBuff))
        {
            evasionBuff = new EvasionBuffEffect(amount, duration);
            effectHandler.AddEffect(evasionBuff);
        }
        else
        {
            evasionBuff.UpdateEffectTime(amount, duration, this);
        }
    }

    //회피 버프 해제
    public void CancelEvasionBuff()
    {
        effectHandler.RemoveEffect(evasionBuff);
    }

    public void ApplySkillValueDebuff(float amount, float duration)
    {
        if (!effectHandler.IsInEffect(skillValueDebuff))
        {
            skillValueDebuff = new SkillValueDebuffEffect(amount, duration);
            effectHandler.AddEffect(skillValueDebuff);
        }
        else
        {
            skillValueDebuff.UpdateEffectTime(amount, duration, this);
        }
    }

    public void CancelSkillValueDebuff()
    {
        effectHandler.RemoveEffect(skillValueDebuff);
    }

    public void ApplySilenceDebuff(float duration)
    {
        if (!effectHandler.IsInEffect(silenceDebuff))
        {
            silenceDebuff = new SilenceDebuff(0, duration);
            effectHandler.AddEffect(silenceDebuff);
        }
        else
        {
            silenceDebuff.UpdateEffectTime(0, duration, this);
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

        transform.DOMove(targetPosition, speed).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            if (!Physics2D.OverlapCircle(this.transform.position, AttackRange, targetLayer))
            {
                isAttack = false;
            }
        });


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
