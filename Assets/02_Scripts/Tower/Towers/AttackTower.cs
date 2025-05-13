using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AdaptedTowerData
{
    public int towerIndex;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public bool bossImmunebuff;
    public  List<int> buffTowerIndex;


    public AdaptedTowerData(int towerIndex, float attackPower, float attackSpeed, float attackRange)
    {
        this.towerIndex = towerIndex;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.bossImmunebuff = false;
        this.attackRange = attackRange;
        buffTowerIndex = new List<int>();
    }
}
public class AttackTower : BaseTower
{

    [Header("공격")]
    [SerializeField] private Transform target;
    private float lastCheckTime = 0f;
    public ProjectileFactory projectileFactory;
    private BaseMonster currentTargetMonster;

    [Header("버프")]
    public AdaptedTowerData adaptedTowerData;
    List<int> buffTowerIndex;
    //private bool Disable;

    [Header("공격속도")]
    float attackSpeedBuff = 1f;
    float windBuff = 1f;
    float windSpeedBuff = 1f;
    public bool isSpeedBuffed = false;
    public bool isWindBuffed = false;

    /// <summary>
    /// 초기화 함수
    /// 자기 자신의 이펙트를 저장한다.
    /// </summary>
    /// <param name="data"></param>
    public override void Init(TowerData data)
    {

        base.Init(data);
        adaptedTowerData = new AdaptedTowerData(towerData.TowerIndex, towerData.AttackPower, towerData.AttackSpeed, towerData.AttackRange);
        OnPlatform();
        projectileFactory = FindObjectOfType<ProjectileFactory>();
        buffTowerIndex = new List<int>();
        if (towerData.SpecialEffect != SpecialEffect.None)
        {
            buffTowerIndex.Add(towerData.TowerIndex);
            adaptedTowerData.buffTowerIndex.Add(towerData.TowerIndex);
        }
        ScanBuffTower();
        ScanPlantedObstacle();
    }

    ///////////=====================주변스캔 및 공격=====================================/////////////////////

    protected override void Update()
    {
        base.Update();
        if (Time.time - lastCheckTime < adaptedTowerData.attackSpeed) return;
        {
            FindTarget();
            if (projectileFactory == null || towerData == null)
            {
                return;  
            }
            Attack();
        }
    }

    bool IsInRange(Vector3 targetPos)
    {
        return Vector3.Distance(transform.position, targetPos) <= adaptedTowerData.attackRange;
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedTowerData.attackRange / 2, LayerMaskData.monster);

        float closestDist = float.MaxValue;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = hit.transform;
            }
        }
        if (target == closest) return;
        if (currentTargetMonster != null)
        {
            currentTargetMonster.OnMonsterDeathAction -= HandleTargetDeath;
        }
        target = closest;
        if(target!=null)currentTargetMonster = target.GetComponent<BaseMonster>();
        if (currentTargetMonster != null)
        {
            currentTargetMonster.OnMonsterDeathAction += HandleTargetDeath;
        }
    }

    void Attack()
    {
        //마스터리포인트 1/1000 ->인게임매니저 ->게임매니저
        if (disable) return;
        //bool isMultyTarget = towerData.SpecialEffect == SpecialEffect.MultyTarget;
        bool isMultyTarget = towerData.EffectTarget == EffectTarget.Multiple;
        bool shouldMultyShot = isMultyTarget && UnityEngine.Random.Range(0f, 1f) < towerData.EffectChance;
        if (target == null || !IsInRange(target.position)) return;
        lastCheckTime = Time.time;
        animator.SetTrigger("TowerActive");
        switch (towerData.ProjectileType)
        {
            case ProjectileType.Blast:
                projectileFactory.SpawnAndLaunch<BlastProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex,environmentEffect);
                break;
            case ProjectileType.Magic:

                if (shouldMultyShot)
                {
                    projectileFactory.MultiSpawnAndLaunch<MagicProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, towerData.EffectTargetCount, environmentEffect);
                }
                else
                {
                    projectileFactory.SpawnAndLaunch<MagicProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, environmentEffect);
                }
                break;
            case ProjectileType.Arrow:
                if (shouldMultyShot)
                {
                    projectileFactory.MultiSpawnAndLaunch<ArrowProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, towerData.EffectTargetCount, environmentEffect);
                }
                else
                {
                    projectileFactory.SpawnAndLaunch<ArrowProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex,environmentEffect);
                }
                break;
            default:
                Debug.LogError($"[BaseTower] {towerData.TowerName} 공격타입 없음");
                break;
        }
    }

    private void HandleTargetDeath()
    {
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격대상 사망");
        target = null;
        lastCheckTime = Time.time;
        currentTargetMonster.OnMonsterDeathAction -= HandleTargetDeath;
        currentTargetMonster = null;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (currentTargetMonster != null)
        {
            currentTargetMonster.OnMonsterDeathAction -= HandleTargetDeath;
            currentTargetMonster = null;
        }
    }

    ///////////=====================버프=====================================/////////////////////

    /// <summary>
    /// 공격력 버프
    /// </summary>
    /// <param name="buff"></param>
    public void AttackPowerBuff(float buff)
    {
        if(towerData.AttackPower + towerData.AttackPower * buff> adaptedTowerData.attackPower)
        adaptedTowerData.attackPower = towerData.AttackPower + towerData.AttackPower * buff;
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격력 증가: {adaptedTowerData.attackPower}");
    }

    /// <summary>
    /// 공격속도 버프
    /// </summary>
    void UpdateAttackSpeed()
    {
        if (isSpeedBuffed && isWindBuffed) windSpeedBuff = 1.2f;
        else windSpeedBuff = 1f;
        float totalBuff = attackSpeedBuff * windBuff* windSpeedBuff;
        adaptedTowerData.attackSpeed = 1f / (towerData.AttackSpeed * totalBuff);
    }
    public void AttackSpeedBuff(float buff)
    {
        if (buff > attackSpeedBuff)
        {
            isSpeedBuffed = true;
            attackSpeedBuff = buff;
            UpdateAttackSpeed();
        }
    }

    public void RemoveAttackSpeedBuff()
    {
        isSpeedBuffed = false;
        attackSpeedBuff = 1f;
        UpdateAttackSpeed();
    }

    public void OnWindSpeedBuff()
    {
        isWindBuffed = true;
        windBuff = 1.2f;
        UpdateAttackSpeed();
    }

    public void OffWindSpeedBuff()
    {
        isWindBuffed = false;
        windBuff = 1f;
        windSpeedBuff = 1f;
        UpdateAttackSpeed();
    }
    public void BossImmuneBuff()
    {
        adaptedTowerData.bossImmunebuff = true;
        Debug.Log($"[BaseTower] {towerData.TowerName} 보스 면역 증가: {adaptedTowerData.bossImmunebuff}");
    }

    public void RemoveAttackPowerBuff()
    {
        adaptedTowerData.attackPower = towerData.AttackPower;
    }

    public void RemoveBossImmuneBuff()
    {
        adaptedTowerData.bossImmunebuff = false;
    }


    public void AddEffect(int towerIndex,EnvironmentEffect environmentEffect)
    {
        Debug.Log($"버프받음,environmentEffect={environmentEffect}");
        if (environmentEffect.isNearFire && TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect == SpecialEffect.DotDamage)
        {
            this.environmentEffect.isBuffAffectedByFire = true;
            Debug.Log("버프타워중에 불옆에있는 타워가있음");
        }

        if (environmentEffect.isNearWater && TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect == SpecialEffect.Slow)
        {
            this.environmentEffect.isBuffAffectedByWater = true;
            Debug.Log("버프타워중에 물옆에있는 타워가있음");
        }
        bool found = false;
        if (buffTowerIndex.Contains(towerIndex)) return;
        for (int i = 0; i < buffTowerIndex.Count; i++)
        {
            if (TowerManager.Instance.GetTowerData(buffTowerIndex[i]).SpecialEffect == TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect)
            {
                var existing = TowerManager.Instance.GetTowerData(buffTowerIndex[i]);
                if (existing.EffectValue < TowerManager.Instance.GetTowerData(towerIndex).EffectValue)
                {
                    buffTowerIndex[i] = towerIndex;
                    adaptedTowerData.buffTowerIndex[i] = towerIndex;
                }
                found = true;
                break;
            }
        }
        if (!found)
        {
            buffTowerIndex.Add(towerIndex);
            adaptedTowerData.buffTowerIndex.Add(towerIndex);
        }
    }

    /// <summary>
    /// 플랫폼 위에 설치 시 사거리 증가
    /// </summary>
    private void OnPlatform()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position, LayerMaskData.platform);
        foreach (var hit in hits)
        {
            if(EnviromentManager.Instance.Season==Season.winter)adaptedTowerData.attackRange = towerData.AttackRange*1.1f;
            else
            adaptedTowerData.attackRange = towerData.AttackRange*1.15f;
            return;
        }
    }
    public override void DestroyBuffTower()
    {
        ClearAllbuff();
        ScanBuffTower();
    }

    private void ClearAllbuff()
    {
        environmentEffect.ClearEffect();
        RemoveBossImmuneBuff();
        RemoveAttackPowerBuff();
        RemoveAttackSpeedBuff();
        buffTowerIndex.Clear();
        buffTowerIndex.Add(towerData.TowerIndex);
    }
}