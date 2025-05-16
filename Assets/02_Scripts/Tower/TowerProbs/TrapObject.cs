using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class AdaptedTrapObjectData
{
    public int towerIndex;
    public float baseEffectValue;
    public float coolTime;
    public float effectValue;
    public float attackRange;
    public float effectDuration;


    public AdaptedTrapObjectData(int towerIndex, float effectValue,float effectDuration)
    {
        this.towerIndex = towerIndex;
        this.baseEffectValue = effectValue;
        this.attackRange = 1f;
        this.effectDuration = effectDuration;
        this.coolTime = effectDuration;
        Upgrade();
        this.effectValue = baseEffectValue;
    }

    //////////////////////////////////////////업그레이드////////////////////////////////////////////////
    public void Upgrade()
    {
        int buffEffectValueupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectValue];
        baseEffectValue *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectValue].levels[buffEffectValueupgradeLevel];


        int buffEffectRangeupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectRange];
        attackRange *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectRange].levels[buffEffectRangeupgradeLevel];

        int buffEffectDurationupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectDuration];
        effectDuration *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectDuration].levels[buffEffectDurationupgradeLevel];

        int buffEffectAttackSpeedupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackSpeed];
        float buffEffectAttackSpeed = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackSpeed].levels[buffEffectAttackSpeedupgradeLevel];
        coolTime = coolTime / buffEffectAttackSpeed;

        int buffEffectCombetMastery = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.CombetMastery];
        float combetMasteryValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.CombetMastery].levels[buffEffectCombetMastery];
        baseEffectValue *= combetMasteryValue;
        attackRange *= combetMasteryValue;
        coolTime = coolTime / combetMasteryValue;
    }
}

public interface ITrapEffect
{
    public void Apply(BaseMonster target,TowerData towerData ,AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect);
}
public enum TrapObjectState
{
    CantActive, 
    Ready,
    Triggered,
    Cooldown
}
public class TrapObject : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<int> buffTowerIndex;
    [SerializeField] private List<ITrapEffect> trapEffectList;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    AdaptedTrapObjectData adaptedTrapObjectData;
    private TowerData towerData;


    public EnvironmentEffect environmentEffect;
    private Coroutine activeEffectCoroutine;
    private Coroutine checkOverlapCoroutine;
    private Animator animator;

    private TrapObjectState currentState;

    private Dictionary<Type, int> currentEffectSources = new();

    private float creationTime;
    private float cooldownTime;

    public bool bossImmunebuff = false;

    private float EmergencyResponseBuff = 1f;

    private float maxbuffRadius = 5f;

    private bool disable = false;  
    public  void Init(TowerData towerData)
    {
        this.towerData = towerData;
        adaptedTrapObjectData = new AdaptedTrapObjectData(towerData.TowerIndex, towerData.EffectValue,towerData.EffectDuration);
        creationTime = Time.time;
        environmentEffect = new EnvironmentEffect();
        buffTowerIndex = new List<int>();
        trapEffectList= new List<ITrapEffect>();
        bossImmunebuff = false;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        Utils.GetColor(towerData, GetComponent<SpriteRenderer>());
        buffTowerIndex.Add(towerData.TowerIndex);
        AddTrapEffect(towerData.TowerIndex);
        ScanBuffTower();
        CanPlant();
    }
    private  void Update()
    {
        if (currentState == TrapObjectState.Cooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                ChageState(TrapObjectState.Ready);
                cooldownTime = adaptedTrapObjectData.coolTime;
            }
        }
    }

    /// <summary>
    /// 트랩에 적용되는 이팩트 리스트
    /// </summary>
    private static readonly Dictionary<SpecialEffect, Type> effectTypeMap = new()
    {
        { SpecialEffect.DotDamage, typeof(TrapObjectDotDamageEffect) },
        { SpecialEffect.Slow, typeof(TrapObjectSlowEffect) },
        { SpecialEffect.Silence, typeof(TrapObjectSilenceEffect) },
        { SpecialEffect.Knockback, typeof(TrapObjectKnockbackEffect) },
        { SpecialEffect.Stun, typeof(TrapObjectStunEffect)},
    };

    /// <summary>
    /// 설치 가능여부 판별
    /// </summary>
    public void CanPlant()
    {
        Vector2 pos = PostionArray();
        Collider2D[] blockHits = Physics2D.OverlapPointAll(pos, LayerMaskData.buildBlockTrap);
        foreach (var hit in blockHits)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Collider2D[] plantedObstcles = Physics2D.OverlapPointAll(pos, LayerMaskData.plantedObstacle);
        foreach (var hit in plantedObstcles)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Collider2D[] floorHits = Physics2D.OverlapPointAll(pos, LayerMaskData.obstacleZone);
        environmentEffect.isNearWater = false;
        environmentEffect.isNearFire = false;
        foreach (var hit in floorHits)
        {
            PlantedEffect plantedEffect = hit.GetComponent<PlantedEffect>();
            switch (plantedEffect.obstacleType)
            {
                case ObstacleType.Water:
                    Debug.Log("설치위치옆에 물있음");
                    environmentEffect.isNearWater = true;
                    break;
                case ObstacleType.Fire:
                    Debug.Log("설치위치옆에 불있음");
                    environmentEffect.isNearFire = true;
                    break;

            }
        }

        Collider2D[] trapHits = Physics2D.OverlapPointAll(pos, LayerMaskData.trapObject);
        checkOverlapCoroutine=StartCoroutine(CheckTrapOverlap(trapHits));
    }

    /// <summary>
    /// 트랩 오브젝트가 겹치는지 체크하는 코루틴 가장 먼저 생성된 트랩활성화된다.
    /// </summary>
    /// <param name="trapHits"></param>
    /// <returns></returns>
    private IEnumerator CheckTrapOverlap(Collider2D[] trapHits)
    {
        yield return null;
        if (this == null) yield break;
        foreach (var hit in trapHits)
        {
            if (hit == null || hit.gameObject == this.gameObject) continue;
            TrapObject other = hit.GetComponent<TrapObject>();
            if (other != null && other.creationTime < this.creationTime && other.currentState != TrapObjectState.CantActive)
            {
                Debug.Log($"충돌체있음{other}");
                ChageState(TrapObjectState.CantActive);
                checkOverlapCoroutine= null;
                yield break;
            }
        }
        checkOverlapCoroutine = null;
        ChageState(TrapObjectState.Ready);
    }


    public bool IsAnyObjectOnTile()
    {
        Collider2D hit = Physics2D.OverlapPoint(PostionArray(), LayerMaskData.buildBlock);
        return hit != null;
    }

    public Vector2 PostionArray()
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    /// <summary>
    /// 트랩에 몬스터가 닿았을때 호출된다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != TrapObjectState.Ready) return;
        if (disable) return;
        if (collision.CompareTag("Monster"))
        {
            BaseMonster monster = collision.GetComponent<BaseMonster>();
            if (monster != null)
            {
                ChageState(TrapObjectState.Triggered);
                if (activeEffectCoroutine != null)
                    StopCoroutine(activeEffectCoroutine);
                activeEffectCoroutine = StartCoroutine(ApplyEffectsToArea(monster));
            }
        }
    }

    /// <summary>
    /// 트랩이팩트 적용
    /// 일정시간동안 이팩트적용 후 쿨타임으로 전환 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator ApplyEffectsToArea(BaseMonster target)
    {
        float elapsed = 0f;
        float applyInterval = 0.1f;

        while (elapsed < towerData.EffectDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedTrapObjectData.attackRange, LayerMaskData.monster);
            foreach (var hit in hits)
            {
                BaseMonster monster = hit.GetComponent<BaseMonster>();
                if (monster == null) continue;
                for(int i=0;i< buffTowerIndex.Count;i++)
                {
                    trapEffectList[i].Apply(monster,TowerManager.Instance.GetTowerData(buffTowerIndex[i]), TowerManager.Instance.GetAdaptedTrapObjectData(buffTowerIndex[i]), bossImmunebuff, environmentEffect);
                    Debug.Log(bossImmunebuff);
                }

            }
            yield return new WaitForSeconds(applyInterval);
            elapsed += applyInterval;
        }
        activeEffectCoroutine = null;
        animator.SetTrigger("TrapObjectDespawn");
        yield return new WaitForSeconds(0.1f);
        animator.enabled = false;
        ChageState(TrapObjectState.Cooldown);
    }

    /// <summary>
    /// 트렙에 적용되는 이팩트 추가
    /// </summary>
    /// <param name="index"></param>
    public void AddTrapEffect(int index)
    {
        TowerData data = TowerManager.Instance.GetTowerData(index);
        SpecialEffect effectKey = data.SpecialEffect;

        if (!effectTypeMap.TryGetValue(effectKey, out var effectType))
        {
            return;
        }

        if (TryGetComponent(effectType, out var existingComp))
        {
            int existingIndex = currentEffectSources.TryGetValue(effectType, out var prevIndex) ? prevIndex : -1;
            if (existingIndex != -1)
            {
                float existingValue = TowerManager.Instance.GetTowerData(existingIndex).EffectValue;
                if (existingValue >= data.EffectValue)
                {
                    return;
                }

                Destroy(existingComp as Component);
                currentEffectSources.Remove(effectType);

                trapEffectList.RemoveAll(e => e?.GetType() == effectType);
            }
        }
        var newEffect = gameObject.AddComponent(effectType) as ITrapEffect;
        if (newEffect != null)
        {
            trapEffectList.Add(newEffect);
            currentEffectSources[effectType] = index;
        }
    }

    public void ApplyEmergencyResponse()
    {
        int emergencyResponseLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.Emergencyresponse];
        EmergencyResponseBuff = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.Emergencyresponse].levels[emergencyResponseLevel];
        adaptedTrapObjectData.effectValue = adaptedTrapObjectData.baseEffectValue * EmergencyResponseBuff;
    }

    public void RemoveEmergencyResponse()
    {
        adaptedTrapObjectData.effectValue = adaptedTrapObjectData.baseEffectValue;
    }


    public void BossImmuneBuff()
    {
        bossImmunebuff = true;
    }
    public void RemoveBossImmuneBuff()
    {
        bossImmunebuff = false;
    }

    public void AddEffect(int targetIndex)
    {
        bool found = false;

        for (int i = 0; i < buffTowerIndex.Count; i++)
        {
            if (buffTowerIndex[i] == targetIndex)
            {
                var existing = TowerManager.Instance.GetTowerData(buffTowerIndex[i]);
                if (existing.EffectValue < TowerManager.Instance.GetTowerData(targetIndex).EffectValue)
                {
                    buffTowerIndex[i] = targetIndex;
                    AddTrapEffect(targetIndex);
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            buffTowerIndex.Add(targetIndex);
            AddTrapEffect(targetIndex);
        }
    }

    public void OnDisabled()
    {
        disable = true;
        Invoke("OnEnabled", 3.5f);
    }
    public void OnEnabled()
    {
        disable = false;
    }

    ///////////==========================상태초기화 함수들================================/////////////////////

    public void DestroyBuffTower()
    {
        ClearAllbuff();
        ScanBuffTower();
    }

    private void ClearAllbuff()
    {
       
        buffTowerIndex.Clear();
        trapEffectList.Clear();
        RemoveBossImmuneBuff();
        buffTowerIndex.Add(towerData.TowerIndex);
        AddTrapEffect(towerData.TowerIndex);
    }

    /// <summary>
    /// 버프타워를 스캔하여 버프를 재적용한다.
    /// </summary>
    private void ScanBuffTower()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxbuffRadius, LayerMaskData.tower);

        foreach (var hit in hits)
        {
            BuffTower otherTower = hit.GetComponent<BuffTower>();
            if (otherTower != null)
            {
                otherTower.ReApplyBuff();
            }
        }
    }

    /// <summary>
    /// 주변에 장애물들을 스캔하여 버프를 재적용한다.
    /// </summary>
    public void ScanPlantedObstacle()
    {
        environmentEffect.isNearWater = false;
        environmentEffect.isNearFire = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position, LayerMaskData.obstacleZone);
        foreach (var hit in hits)
        {
            PlantedEffect plantedEffect = hit.GetComponent<PlantedEffect>();
            switch (plantedEffect.obstacleType)
            {
                case ObstacleType.Water:
                    Debug.Log("설치위치옆에 물있음");
                    environmentEffect.isNearWater = true;
                    break;
                case ObstacleType.Fire:
                    Debug.Log("설치위치옆에 불있음");
                    environmentEffect.isNearFire = true;
                    break;

            }
        }
    }
    public void OnActive()
    {
        sr.enabled = true;
    }

    public void UnActive()
    {
        sr.enabled = false;
    }

    public void ChageState(TrapObjectState trapObjectState)
    {
        currentState = trapObjectState;
        switch (trapObjectState)
        {
            case TrapObjectState.CantActive:
                animator.enabled = false;
                UnActive();
                break;
            case TrapObjectState.Ready:
                animator.enabled = true;
                CanPlant();
                OnActive();
                break;
            case TrapObjectState.Triggered:
                animator.SetTrigger("TrapObjectActive");
                break;
            case TrapObjectState.Cooldown:
                UnActive();
                break;
        }
    }


    private void OnDestroy()
    {
        if (checkOverlapCoroutine != null)
        {
            StopCoroutine(checkOverlapCoroutine);
            checkOverlapCoroutine = null;
        }
        if (activeEffectCoroutine != null)
        {
            StopCoroutine(activeEffectCoroutine);
            activeEffectCoroutine = null;
        }
        if (isActiveAndEnabled)TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
    }
}
