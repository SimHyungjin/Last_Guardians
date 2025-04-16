using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class AttackTower : BaseTower
{

    [Header("����")]
    [SerializeField] private Transform target;
    private float lastCheckTime = 0f;
    [SerializeField] private LayerMask monsterLayer;
    public ProjectileFactory projectileFactory;
    //������� -> ���丮�� ����
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
    }
    protected override void Update()
    {
        base.Update();
        if (Time.time - lastCheckTime < towerData.AttackSpeed) return;
        {
            FindTarget();
            if (projectileFactory == null || towerData == null)
            {
                Debug.LogError("ProjectileFactory or TowerData is null in BaseTower.Update");
                return;  // �ʼ� ��ü�� null�̶�� Update���� �� �̻� �������� ����
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
        currentTargetMonster = target.GetComponent<BaseMonster>();
        if (currentTargetMonster != null)
        {
            currentTargetMonster.OnMonsterDeathAction += HandleTargetDeath;
        }
    }

    void Attack()
    {
        if (target == null || !IsInRange(target.position)) return;
        Debug.Log($"[BaseTower] {towerData.TowerName} ���ݴ��: {target.name}");
        //projectileFactory.SpawnAndLaunch(target.position,towerData,this.transform);
        switch (towerData.ProjectileType)
        {
            case ProjectileType.Magic:
                projectileFactory.SpawnAndLaunch<MagicProjectile>(target.position, towerData, this.transform);
                break;
            case ProjectileType.Blast:
                projectileFactory.SpawnAndLaunch<BlastProjectile>(target.position, towerData, this.transform);
                break;
            case ProjectileType.Arrow:
                projectileFactory.SpawnAndLaunch<ArrowProjectile>(target.position, towerData, this.transform);
                break;
            default:
                Debug.LogError($"[BaseTower] {towerData.TowerName} ����Ÿ�� ����");
                break;
        }
    }
    private void HandleTargetDeath()
    {
        Debug.Log($"[BaseTower] {towerData.TowerName} ���ݴ�� ���");
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
        Debug.Log($"[BaseTower] {towerData.TowerName} ���ݷ� ����: {attackPower}");
    }
    public void AttackSpeedBuff(float buff)
    {
        attackSpeed = attackSpeed /((1f+buff));
        Debug.Log($"[BaseTower] {towerData.TowerName} ���ݼӵ� ����: {attackSpeed}");
    }
}