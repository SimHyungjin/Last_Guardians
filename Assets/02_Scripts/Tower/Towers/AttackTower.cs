using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[Serializable]
public class AdaptedAttackTowerData
{

    [Header("공격마스터리")]
    public int towerIndex;
    public float baseAttackPower;
    public float baseattackSpeed;
    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public int projectileCount;

    [Header("효과마스터리")]
    public float effectValue;
    public float effectDuration;


    [Header("타워버프")]
    public bool bossImmunebuff;
    public  List<int> buffTowerIndex;


    public AdaptedAttackTowerData(int towerIndex, float attackPower, float attackSpeed, float attackRange, int projectileCount, float effectValue ,float effectDuration)
    {
        this.towerIndex = towerIndex;
        this.baseAttackPower = attackPower;
        this.baseattackSpeed = attackSpeed;
        this.bossImmunebuff = false;
        this.attackRange = attackRange;
        this.projectileCount = projectileCount;

        this.effectValue = effectValue;
        this.effectDuration = effectDuration;

        Upgrade();
        buffTowerIndex = new List<int>();
        this.attackSpeed = baseattackSpeed;
    }

    //////////////////////////////////////////업그레이드////////////////////////////////////////////////
    public void Upgrade()
    {
        int attackPowerupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackPower];
        baseAttackPower *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackPower].levels[attackPowerupgradeLevel];

        int attackSpeedupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackSpeed];
        float attackSpeedUpgradeValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackSpeed].levels[attackSpeedupgradeLevel];
        baseattackSpeed = baseattackSpeed / attackSpeedUpgradeValue;

        int AttackRangeupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackRange];
        attackRange *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackRange].levels[AttackRangeupgradeLevel];

        int CombetMasteryupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.CombetMastery];
        float CombetMasteryupgradeValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.CombetMastery].levels[CombetMasteryupgradeLevel];
        baseAttackPower *= CombetMasteryupgradeValue;
        baseattackSpeed = baseattackSpeed / CombetMasteryupgradeValue;
        attackRange *= CombetMasteryupgradeValue;

        int MultipleAttackLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.MultipleAttack];
        projectileCount += (int)TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.MultipleAttack].levels[MultipleAttackLevel];

        int EffectValueLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectValue];
        effectValue *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectValue].levels[EffectValueLevel];
        int EffectDurationLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectDuration];
        effectDuration *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectDuration].levels[EffectDurationLevel];

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
    public AdaptedAttackTowerData adaptedTowerData;
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
    private float bossSlayerBuff = 1f;
    private bool ContinuousAttack = false;
    /// <summary>
    /// 초기화 함수
    /// 자기 자신의 이펙트를 저장한다.
    /// </summary>
    /// <param name="data"></param>
    public override void Init(TowerData data)
    {

        base.Init(data);
        adaptedTowerData = new AdaptedAttackTowerData(towerData.TowerIndex, towerData.AttackPower, towerData.AttackSpeed, towerData.AttackRange, projectileCount(),towerData.EffectValue,towerData.EffectDuration);
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

    /// <summary>
    /// 최종 공격력 계산식
    /// </summary>
    private void CalculateDamage()
    {
        adaptedTowerData.attackPower = adaptedTowerData.baseAttackPower * attackPowerBuff * ContinuousAttackBuff * bossSlayerBuff;
    }

    /// <summary>
    /// 공격 메서드
    /// </summary>
    void Attack()
    {
        if (disable) return;
        Debug.Log($"[BaseTower]공격력 {adaptedTowerData.attackPower} ");
        //bool shouldMultyShot = isMultyTarget && UnityEngine.Random.Range(0f, 1f) < towerData.EffectChance;
        if (target == null || !IsInRange(target.position)) return;
        int modifyProjectileCount = ModifyProjectileCount();

        lastCheckTime = Time.time;
        animator.SetTrigger("TowerActive");
        switch (towerData.ProjectileType)
        {
            case ProjectileType.Blast:
                projectileFactory.MultiSpawnAndLaunch<BlastProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, modifyProjectileCount, environmentEffect);
                break;
            case ProjectileType.Magic:

                projectileFactory.MultiSpawnAndLaunch<MagicProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, modifyProjectileCount, environmentEffect);
                break;
            case ProjectileType.Arrow:
                projectileFactory.MultiSpawnAndLaunch<ArrowProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, modifyProjectileCount, environmentEffect);
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

    //////////////////////////////////////////타워 업그레이드////////////////////////////////////////////////

    /// <summary>
    /// 연속 공격
    /// </summary>
    void StartContinuousAttack()
    {
        ContinuousAttack = true;
        int ContinuousAttackupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.ContinuousAttack];
        ContinuousAttackBuff= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.ContinuousAttack].levels[ContinuousAttackupgradeLevel];
        CalculateDamage();
    }
    void StopContinuousAttack()
    {
        ContinuousAttack = false;
        ContinuousAttackBuff = 1f;
        CalculateDamage();
    }

    public void BossSlayerBuff()
    {
        int BossSlayerupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.BossSlayer];
        float bossSlayerBuffvalue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.BossSlayer].levels[BossSlayerupgradeLevel];
        Debug.Log($"[BaseTower] {towerData.TowerName} 보스킬 증가: {bossSlayerBuffvalue}  몬스터 킬수: {MonsterManager.Instance.BossKillCount}");
        bossSlayerBuff = 1+(bossSlayerBuffvalue*Mathf.Min(MonsterManager.Instance.BossKillCount,10));
        CalculateDamage();
    }


    /// <summary>
    /// 발사체 수 초기화
    /// </summary>
    /// <returns></returns>
    private int projectileCount()
    {
        if(towerData.EffectTarget==EffectTarget.Multiple)
        {
            return towerData.EffectTargetCount;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// 발사체 수 조정
    /// </summary>
    /// <returns></returns>
    private int ModifyProjectileCount()
    {
        if (towerData.ElementType != ElementType.Wind || towerData.SpecialEffect != SpecialEffect.MultyTarget)
        {
            return adaptedTowerData.projectileCount;
        }

        return UnityEngine.Random.Range(0f, 1f) < towerData.EffectChance
            ? adaptedTowerData.projectileCount
            : adaptedTowerData.projectileCount - (towerData.EffectTargetCount-1);
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
        adaptedTowerData.attackSpeed = adaptedTowerData.baseattackSpeed / totalBuff;
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