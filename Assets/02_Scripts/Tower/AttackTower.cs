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
    public List<int> buffTowerIndex;
    public bool bossImmune;

    public AdaptedTowerData(int towerIndex, float attackPower, float attackSpeed, bool bossImmune)
    {
        this.towerIndex = towerIndex;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.bossImmune = bossImmune;
    }
    public void subSelfEffect()
    {
        if (buffTowerIndex != null)
        {
            foreach (int towerIndex in buffTowerIndex)
            {
                if (towerIndex == this.towerIndex)
                {
                    buffTowerIndex.Remove(towerIndex);
                    break;
                }
            }
        }
    }
}
public class AttackTower : BaseTower
{

    [Header("공격")]
    [SerializeField] private Transform target;
    private float lastCheckTime = 0f;
    [SerializeField] private LayerMask monsterLayer;
    public ProjectileFactory projectileFactory;

    List<int> buffTowerIndex;
    private BaseMonster currentTargetMonster;

    public AdaptedTowerData adaptedTowerData;
    public override void Init(TowerData data)
    {
        base.Init(data);
        monsterLayer = LayerMask.GetMask("Monster");
        projectileFactory = FindObjectOfType<ProjectileFactory>();
        buffTowerIndex = new List<int>();
        if (towerData.SpecialEffect != SpecialEffect.None)
        {
            buffTowerIndex.Add(towerData.TowerIndex);
        }
        adaptedTowerData = new AdaptedTowerData(towerData.TowerIndex, towerData.AttackPower, towerData.AttackSpeed, towerData.BossImmune);

    }
    protected override void Update()
    {
        base.Update();
        if (Time.time - lastCheckTime < adaptedTowerData.attackSpeed) return;
        {
            FindTarget();
            if (projectileFactory == null || towerData == null)
            {
                Debug.LogError("ProjectileFactory or TowerData is null in BaseTower.Update");
                return;  // 필수 객체가 null이라면 Update에서 더 이상 진행하지 않음
            }
            lastCheckTime = Time.time;
            Attack();
        }
    }

    bool IsInRange(Vector3 targetPos)
    {
        return Vector3.Distance(transform.position, targetPos) <= towerData.AttackRange;
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, monsterLayer);

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

        bool isMultyTarget = towerData.SpecialEffect == SpecialEffect.MultyTarget;
        bool shouldMultyShot = isMultyTarget && UnityEngine.Random.Range(0f, 1f) < towerData.EffectChance;
        if (target == null || !IsInRange(target.position)) return;
        //projectileFactory.SpawnAndLaunch(target.position,towerData,this.transform);
        switch (towerData.ProjectileType)
        {
            case ProjectileType.Blast:
                projectileFactory.SpawnAndLaunch<BlastProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex);
                break;
            case ProjectileType.Magic:

                if (shouldMultyShot)
                {
                    projectileFactory.MultiSpawnAndLaunch<MagicProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, towerData.EffectTargetCount);
                }
                else
                {
                    projectileFactory.SpawnAndLaunch<MagicProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex);
                }
                break;

            case ProjectileType.Arrow:
                if (shouldMultyShot)
                {
                    projectileFactory.MultiSpawnAndLaunch<ArrowProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex, towerData.EffectTargetCount);
                }
                else
                {
                    projectileFactory.SpawnAndLaunch<ArrowProjectile>(target.position, towerData, adaptedTowerData, this.transform, buffTowerIndex);
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

    public void AttackPowerBuff(float buff)
    {
        adaptedTowerData.attackPower = towerData.AttackPower + towerData.AttackPower * buff;
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격력 증가: {adaptedTowerData.attackPower}");
    }
    public void AttackSpeedBuff(float buff)
    {
        adaptedTowerData.attackSpeed = towerData.AttackSpeed / buff;
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격속도 증가: {adaptedTowerData.attackSpeed}");
    }

    public void BossImmuneBuff(bool buff)
    {
        adaptedTowerData.bossImmune = buff;
        Debug.Log($"[BaseTower] {towerData.TowerName} 보스 면역 증가: {adaptedTowerData.bossImmune}");
    }

    public void RemoveAttackPowerBuff()
    {
        adaptedTowerData.attackPower = towerData.AttackPower;
    }
    public void RemoveAttackSpeedBuff()
    {
        adaptedTowerData.attackSpeed = towerData.AttackSpeed;
    }
    public void RemoveBossImmuneBuff()
    {
        adaptedTowerData.bossImmune = false;
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
                    adaptedTowerData.buffTowerIndex[i] = targetIndex;
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            buffTowerIndex.Add(targetIndex);
            adaptedTowerData.buffTowerIndex.Add(targetIndex);
        }
    }
}