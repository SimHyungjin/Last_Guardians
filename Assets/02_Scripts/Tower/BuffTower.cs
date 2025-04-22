using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITowerBuff
{
    void ApplyBuffToTower(BaseTower tower, TowerData data);
    void ApplyBuffToTrap(TrapObject trap, TowerData data);
    void ApplyDebuff(BaseMonster monster, TowerData data);
}

public class BuffTower : BaseTower
{
    [Header("버프타워 데이터")]
    [SerializeField] private LayerMask monsterLayer;
    public ITowerBuff towerBuff;
    public ITowerBuff monsterDebuff;

    public List<int> buffTowerIndex;
    public List<ITowerBuff> buffMonterDebuffs;
    private float lastCheckTime = 0f;

    public override void Init(TowerData data)
    {
        base.Init(data);
        towerLayer = LayerMask.GetMask("Tower");
        monsterLayer = LayerMask.GetMask("Monster");
        buffTowerIndex = new List<int>();
        buffMonterDebuffs = new List<ITowerBuff>();
        BuffSelect(data);
        ScanBuffTower();
        ApplyBuffOnPlacement();
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time - lastCheckTime < 0.1f) return;
        if (towerData.EffectTarget == EffectTarget.All&&!disable) ApplyDebuffOnPlacement();
        if (buffTowerIndex.Count > 0 && towerData.EffectTarget == EffectTarget.All && !disable) ApplyDebuffOnPlacementOnBuff();


        lastCheckTime = Time.time;
    }

    private void BuffSelect(TowerData data)
    {
        if(data.EffectTarget == EffectTarget.Towers)
        {
            switch(data.SpecialEffect)
            {
                case SpecialEffect.AttackPower:
                    towerBuff = new TowerBuffAttackPower();
                    break;
                case SpecialEffect.AttackSpeed:
                    towerBuff = new TowerBuffAttackSpeed();
                    break;
                default:
                    towerBuff = new TowerBuffAddProjectileComponent();
                    break;
            }
        }
        if(data.EffectTarget == EffectTarget.All)
        {
            switch(data.SpecialEffect)
            {
                case SpecialEffect.DotDamage:
                    monsterDebuff = new TowerBuffMonsterDotDamage();
                    break;
                case SpecialEffect.Slow:
                    monsterDebuff = new TowerBuffMonsterSlow();
                    break;
                case SpecialEffect.DefReduc:
                    monsterDebuff = new TowerBuffMonsterReducionDef();
                    break;
            }
        }
    }
    private ITowerBuff BuffAdd(int towerIndex)
    {
        ITowerBuff buff = null;
        switch (TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect)
        {
            case SpecialEffect.DotDamage:
                buff = new TowerBuffMonsterDotDamage();         
                break;
            case SpecialEffect.Slow:
                buff = new TowerBuffMonsterSlow();
                break;
            case SpecialEffect.DefReduc:
                buff = new TowerBuffMonsterReducionDef();
                break;
        }
        return buff;
    }
    private void ApplyBuffOnPlacement()
    {
        if (towerData.EffectTarget != EffectTarget.Towers) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange/2, towerLayer);

        foreach (var hit in hits)
        {
            BaseTower otherTower = hit.GetComponent<BaseTower>();
            if (otherTower != null && otherTower != this)
            {
                towerBuff.ApplyBuffToTower(otherTower, towerData);
            }
        }
        foreach (var hit in hits)
        {
            TrapObject otherTrap = hit.GetComponent<TrapObject>();
            if (otherTrap != null && otherTrap != this)
            {
                towerBuff.ApplyBuffToTrap(otherTrap, towerData);
            }
        }
    }

    private void ApplyDebuffOnPlacement()
    {
        if(towerData.EffectTarget != EffectTarget.All) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange/2, monsterLayer);

        foreach (var hit in hits)
        {
            BaseMonster otherMonster = hit.GetComponent<BaseMonster>();
            if (otherMonster != null && otherMonster != this)
            {
                monsterDebuff.ApplyDebuff(otherMonster, towerData);
            }
        }
    }
    private void ApplyDebuffOnPlacementOnBuff()
    {
        if (towerData.EffectTarget != EffectTarget.All) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange / 2, monsterLayer);

        foreach (var hit in hits)
        {
            BaseMonster otherMonster = hit.GetComponent<BaseMonster>();
            if (otherMonster != null && otherMonster != this)
            {
                for(int i=0; i<buffMonterDebuffs.Count();i++)
                {
                    Debug.Log($"buffTowerIndex.Count = {buffTowerIndex.Count}, buffMonterDebuffs.Count = {buffMonterDebuffs.Count}");
                    buffMonterDebuffs[i].ApplyDebuff(otherMonster, TowerManager.Instance.GetTowerData(buffTowerIndex[i]));
                }
            }
        }
    }
    public void ReApplyBuff()
    {
        ApplyBuffOnPlacement();
    }

    public void AddEffect(int towerIndex)
    {
        bool found = false;
        if (towerIndex == towerData.TowerIndex) return;
        if (buffTowerIndex.Contains(towerIndex)) return;
        for (int i = 0; i < buffTowerIndex.Count; i++)
        {
            if (TowerManager.Instance.GetTowerData(buffTowerIndex[i]).SpecialEffect == TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect)
            {
                var existing = TowerManager.Instance.GetTowerData(buffTowerIndex[i]);
                if (existing.EffectValue < TowerManager.Instance.GetTowerData(towerIndex).EffectValue)
                {
                    buffTowerIndex[i] = towerIndex;
                    ITowerBuff newBuff = BuffAdd(towerIndex);
                    buffMonterDebuffs[i] = newBuff;
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            buffTowerIndex.Add(towerIndex);
            buffMonterDebuffs.Add(BuffAdd(towerIndex));
        }
    }
    public override void DestroyBuffTower()
    {
        ClearAllbuff();
        ScanBuffTower();
    }
    private void ClearAllbuff()
    {
        buffTowerIndex.Clear();
        buffMonterDebuffs.Clear();
    }
    protected override void OnDestroy()
    {
        if (towerData.EffectTarget != EffectTarget.Towers) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange / 2, towerLayer);

        foreach (var hit in hits)
        {
            BaseTower otherTower = hit.GetComponent<BaseTower>();
            if (otherTower != null && otherTower != this)
            {
                otherTower.DestroyBuffTower();
            }
        }
        foreach (var hit in hits)
        {
            TrapObject otherTrap = hit.GetComponent<TrapObject>();
            if (otherTrap != null && otherTrap != this)
            {
                otherTrap.DestroyBuffTower();
            }
        }
    }


}
