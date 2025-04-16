using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerBuff
{
    void ApplyBuff(BaseTower tower, TowerData data);
    void ApplyDebuff(BaseMonster monster, TowerData data);
}

public class BuffTower : BaseTower
{
    [Header("버프타워 데이터")]
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask monsterLayer;
    public ITowerBuff towerBuff;
    public ITowerBuff monsterDebuff;

    private float lastCheckTime = 0f;

    public override void Init(TowerData data)
    {
        base.Init(data);
        towerLayer = LayerMask.GetMask("Tower");
        //ApplyBuffOnPlacement();
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time - lastCheckTime < 0.1f)
            return;
        if (towerData.EffectTarget == EffectTarget.All)
            ApplyDebuffOnPlacement();
        lastCheckTime = Time.time;
    }

    private void ApplyBuffOnPlacement()
    {
        if (towerData.EffectTarget != EffectTarget.Towers) return;
        List<BaseTower> nearbyTowers = new();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, towerLayer);

        foreach (var hit in hits)
        {
            BaseTower otherTower = hit.GetComponent<BaseTower>();
            if (otherTower != null && otherTower != this)
            {
                nearbyTowers.Add(otherTower);
                towerBuff.ApplyBuff(otherTower, towerData);
            }
        }
        Debug.Log($"[BuffTower] 주변 타워 {nearbyTowers.Count}개 발견");
    }

    private void ApplyDebuffOnPlacement()
    {
        if(towerData.EffectTarget != EffectTarget.All) return;
        List<BaseMonster> nearbyTowers = new();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, monsterLayer);

        foreach (var hit in hits)
        {
            BaseMonster otherMonster = hit.GetComponent<BaseMonster>();
            if (otherMonster != null && otherMonster != this)
            {
                nearbyTowers.Add(otherMonster);
                monsterDebuff.ApplyDebuff(otherMonster, towerData);
            }
        }
        Debug.Log($"[BuffTower] 주변 몬스터 {nearbyTowers.Count}개 발견");
    }
}
