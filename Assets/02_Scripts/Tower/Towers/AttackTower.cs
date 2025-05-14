using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[Serializable]
public class AdaptedTowerData
{
    public int towerIndex;
    public float baseAttackPower;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public bool bossImmunebuff;
    public  List<int> buffTowerIndex;


    public AdaptedTowerData(int towerIndex, float attackPower, float attackSpeed, float attackRange)
    {
        this.towerIndex = towerIndex;
        this.baseAttackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.bossImmunebuff = false;
        this.attackRange = attackRange;
        Upgrade();
        buffTowerIndex = new List<int>();
    }
    //////////////////////////////////////////업그레이드////////////////////////////////////////////////
    public void Upgrade()
    {
        int attackPowerupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackPower];
        baseAttackPower *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackPower].levels[attackPowerupgradeLevel];
        Debug.Log($"어택파워업그레이드가{attackPowerupgradeLevel}이라서{TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackPower].levels[attackPowerupgradeLevel]}만큼 수치올렷다 ");
    
        int attackSpeedupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackSpeed];
        //attackSpeed = attackSpeed / (TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackSpeed].levels[attackSpeedupgradeLevel] * attackSpeed);
        float attackSpeedUpgradeValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackSpeed].levels[attackSpeedupgradeLevel];
        attackSpeed = attackSpeed / attackSpeedUpgradeValue;
        Debug.Log($"어택스피드업그레이드가{attackSpeedupgradeLevel}이라서{TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackSpeed].levels[attackSpeedupgradeLevel]}만큼 수치올렷다 ");
    
        int AttackRangeupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackRange];
        attackRange *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackRange].levels[AttackRangeupgradeLevel];
        Debug.Log($"어택레인지업그레이드가{AttackRangeupgradeLevel}이라서{TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackRange].levels[AttackRangeupgradeLevel]}만큼 수치올렷다 ");
        
        int CombetMasteryupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.CombetMastery];
        float CombetMasteryupgradeValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.CombetMastery].levels[CombetMasteryupgradeLevel];
        Debug.Log($"콤벳마스터리업그레이드가{CombetMasteryupgradeLevel}이라서{TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.CombetMastery].levels[CombetMasteryupgradeLevel]}만큼 수치올렷다 ");
        baseAttackPower *= CombetMasteryupgradeValue;
        attackSpeed = attackSpeed / CombetMasteryupgradeValue;
        attackRange *= CombetMasteryupgradeValue;

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

    [Header("공격력 계산")]
    private float attackPowerBuff = 1f;
    private float ContinuousAttackBuff = 1f;
    private bool ContinuousAttack = false;
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
            if(target!=null)Attack();
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
        if (target == closest)
        {
            if (!ContinuousAttack)
            {
                StartContinuousAttack();
                return;
            }
        }
        else 
        {
            if(ContinuousAttack)
            {
                StopContinuousAttack();
            }
        }
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
    void StartContinuousAttack()
    {
        ContinuousAttack = true;
        int ContinuousAttackupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.ContinuousAttack];
        ContinuousAttackBuff= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.ContinuousAttack].levels[ContinuousAttackupgradeLevel];
        CalculateDamage();
        Debug.Log($"연속타격업그레이드가{ContinuousAttackupgradeLevel}이라서{TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackPower].levels[ContinuousAttackupgradeLevel]}만큼 수치올렷다 ");

    }
    void StopContinuousAttack()
    {
        ContinuousAttack = false;
        ContinuousAttackBuff = 1f;
        CalculateDamage();
    }

    private void CalculateDamage()
    {
        adaptedTowerData.attackPower = adaptedTowerData.baseAttackPower * attackPowerBuff * ContinuousAttackBuff;
    }
    void Attack()
    {
        if (disable) return;
        Debug.Log($"[BaseTower]공격력 {adaptedTowerData.attackPower} ");
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
        Debug.Log("버프타워설치");
        if((float)1+buff> attackPowerBuff) attackPowerBuff = (float)1 + buff;
        CalculateDamage();
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격력 증가: {adaptedTowerData.attackPower}");
    }
    public void RemoveAttackPowerBuff()
    {
        attackPowerBuff = 1f;
        CalculateDamage();
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