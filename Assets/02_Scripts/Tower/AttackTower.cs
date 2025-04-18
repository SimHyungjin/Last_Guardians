using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AttackTower : BaseTower
{

    [Header("공격")]
    [SerializeField] private Transform target;
    private float lastCheckTime = 0f;
    [SerializeField] private LayerMask monsterLayer;
    public ProjectileFactory projectileFactory;
    List<int> buffTowerIndex;
    private BaseMonster currentTargetMonster;
    public float attackPower;
    public float attackSpeed;
    public override void Init(TowerData data)
    {
        base.Init(data);
        monsterLayer = LayerMask.GetMask("Monster");
        projectileFactory = FindObjectOfType<ProjectileFactory>();
        attackPower = towerData.AttackPower;
        attackSpeed = towerData.AttackSpeed;
        buffTowerIndex = new List<int>();
        if (towerData.SpecialEffect != SpecialEffect.None)
        {
            buffTowerIndex.Add(towerData.TowerIndex);
        }
    }
    protected override void Update()
    {
        base.Update();
        if (Time.time - lastCheckTime < attackSpeed) return;
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
        if (target == null || !IsInRange(target.position)) return;
        //projectileFactory.SpawnAndLaunch(target.position,towerData,this.transform);
        switch (towerData.ProjectileType)
        {
            case ProjectileType.Magic:
                projectileFactory.SpawnAndLaunch<MagicProjectile>(target.position, towerData, this.transform, buffTowerIndex);
                break;
            case ProjectileType.Blast:
                projectileFactory.SpawnAndLaunch<BlastProjectile>(target.position, towerData, this.transform, buffTowerIndex);
                break;
            case ProjectileType.Arrow:
                projectileFactory.SpawnAndLaunch<ArrowProjectile>(target.position, towerData, this.transform, buffTowerIndex);
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
        attackPower += attackPower*buff;
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격력 증가: {attackPower}");
    }
    public void AttackSpeedBuff(float buff)
    {
        attackSpeed = attackSpeed /((1f+buff));
        Debug.Log($"[BaseTower] {towerData.TowerName} 공격속도 증가: {attackSpeed}");
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
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            buffTowerIndex.Add(targetIndex);
        }
    }

    public void RemoveEffect(int targetIndex)
    {
        var targetData = TowerManager.Instance.GetTowerData(targetIndex);
        for (int i = 0; i < buffTowerIndex.Count; i++)
        {
            var currentData = TowerManager.Instance.GetTowerData(buffTowerIndex[i]);
            if (buffTowerIndex[i] == targetIndex && currentData.EffectValue == targetData.EffectValue)
            {
                buffTowerIndex.Remove(i);
                break;
            }
        }
    }
}