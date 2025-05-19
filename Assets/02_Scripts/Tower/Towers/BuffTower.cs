using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class AdaptedBuffTowerData
{
    public int towerIndex;
    public float baseEffectValue;
    public float coolTime;
    public float effectValue;
    public float attackRange;
    public float effectDuration;


    public AdaptedBuffTowerData(int towerIndex, float effectValue, float attackRange, float effectDuration)
    {
        this.towerIndex = towerIndex;
        this.baseEffectValue = effectValue;
        this.attackRange = attackRange;
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

        int buffEffectRangeupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackRange];
        attackRange *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackRange].levels[buffEffectRangeupgradeLevel];

        int buffEffectDurationupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectDuration];
        effectDuration *= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectDuration].levels[buffEffectDurationupgradeLevel];

        int buffEffectAttackSpeedupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackSpeed];
        float buffEffectAttackSpeed = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackSpeed].levels[buffEffectAttackSpeedupgradeLevel];
        coolTime = coolTime / buffEffectAttackSpeed;

        int buffEffectCombetMastery = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.CombetMastery];
        float combetMasteryValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.CombetMastery].levels[buffEffectCombetMastery];
        baseEffectValue *= combetMasteryValue;
        attackRange *= combetMasteryValue;
        coolTime =coolTime / combetMasteryValue;
    }

}
public interface ITowerBuff
{
    void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData data,EnvironmentEffect environmentEffect);
    void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect);
    void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect);
}

public class BuffTower : BaseTower
{
    [Header("버프타워 데이터")]
    public ITowerBuff towerBuff;
    public ITowerBuff monsterDebuff;

    public List<int> buffTowerIndex;
    public List<ITowerBuff> buffMonterDebuffs;
    private float lastCheckTime = 0f;

    [Header("업그레이드")]
    public AdaptedBuffTowerData adaptedBuffTowerData;
    private float EmergencyResponseBuff = 1f;

    public override void Init(TowerData data)
    {
        base.Init(data);
        adaptedBuffTowerData = new AdaptedBuffTowerData(data.TowerIndex, data.EffectValue, data.AttackRange, data.EffectDuration);
        buffTowerIndex = new List<int>();
        buffMonterDebuffs = new List<ITowerBuff>();
        BuffSelect(data);
        ScanBuffTower();
        ApplyBuffOnPlacement();
        OnPlatform();
    }

    protected override void Update()
    {
        base.Update();
        if (towerData.EffectTarget == EffectTarget.All&&Time.time - lastCheckTime < adaptedBuffTowerData.coolTime) return;
        {
            if (buffTowerIndex.Count > 0 && towerData.EffectTarget == EffectTarget.All && !disable) ApplyDebuffOnPlacementOnBuff();
            if (towerData.EffectTarget == EffectTarget.All && !disable) ApplyDebuffOnPlacement();
        }
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
        if (towerData.EffectTarget != EffectTarget.Towers) return; int combinedLayerMask = LayerMask.GetMask("Tower", "TrapObject");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedBuffTowerData.attackRange / 2, combinedLayerMask);

        foreach (var hit in hits)
        {
            Debug.Log($"hit = {hit}");
            BaseTower otherTower = hit.GetComponent<BaseTower>();
            if (otherTower != null && otherTower != this)
            {
                towerBuff.ApplyBuffToTower(otherTower, adaptedBuffTowerData,environmentEffect);
            }
        }     
        foreach (var hit in hits)
        {
            Debug.Log($"hit = {hit}");
            TrapObject otherTrap = hit.GetComponent<TrapObject>();
            if (otherTrap != null) towerBuff.ApplyBuffToTrap(otherTrap, adaptedBuffTowerData, environmentEffect);
        }
    }

    private void ApplyDebuffOnPlacement()
    {
        if(towerData.EffectTarget != EffectTarget.All) return;
        animator.SetTrigger("TowerActive");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedBuffTowerData.attackRange / 2, LayerMaskData.monster);

        foreach (var hit in hits)
        {
            BaseMonster otherMonster = hit.GetComponent<BaseMonster>();
            if (otherMonster != null && otherMonster != this)
            {
                monsterDebuff.ApplyDebuff(otherMonster, adaptedBuffTowerData, environmentEffect);
            }
        }
    }
    private void ApplyDebuffOnPlacementOnBuff()
    {
        if (towerData.EffectTarget != EffectTarget.All) return;
        animator.SetTrigger("TowerActive");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedBuffTowerData.attackRange / 2, LayerMaskData.monster);

        foreach (var hit in hits)
        {
            BaseMonster otherMonster = hit.GetComponent<BaseMonster>();
            if (otherMonster != null && otherMonster != this)
            {
                for(int i=0; i<buffMonterDebuffs.Count();i++)
                {
                    Debug.Log($"buffTowerIndex.Count = {buffTowerIndex.Count}, buffMonterDebuffs.Count = {buffMonterDebuffs.Count}");
                    buffMonterDebuffs[i].ApplyDebuff(otherMonster, TowerManager.Instance.GetAdaptedBuffTowerData(buffTowerIndex[i]), environmentEffect);
                }
            }
        }
    }
    public void ReApplyBuff()
    {
        ApplyBuffOnPlacement();
    }

    public void AddEffect(int towerIndex,EnvironmentEffect environmentEffect)
    {
        if (environmentEffect.isNearFire && TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect == SpecialEffect.DotDamage) this.environmentEffect.isBuffAffectedByFire = true;
        else this.environmentEffect.isBuffAffectedByFire = false;
        if (environmentEffect.isNearWater && TowerManager.Instance.GetTowerData(towerIndex).SpecialEffect == SpecialEffect.Slow) this.environmentEffect.isBuffAffectedByWater = true;
        else this.environmentEffect.isBuffAffectedByWater = false;
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

    public override void ApplyEmergencyResponse()
    {
        base.ApplyEmergencyResponse();
        int emergencyResponseLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.Emergencyresponse];
        EmergencyResponseBuff = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.Emergencyresponse].levels[emergencyResponseLevel];
        adaptedBuffTowerData.effectValue = adaptedBuffTowerData.baseEffectValue * EmergencyResponseBuff;
    }

    public override void RemoveEmergencyResponse()
    {
        base.RemoveEmergencyResponse();
        adaptedBuffTowerData.effectValue = adaptedBuffTowerData.baseEffectValue;
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

    private void OnPlatform()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position, LayerMaskData.platform);
        foreach (var hit in hits)
        {            
            if(EnviromentManager.Instance.Season==Season.winter)
            {
                adaptedBuffTowerData.attackRange = towerData.AttackRange*1.1f;
            }
            else
                adaptedBuffTowerData.attackRange = towerData.AttackRange * 1.15f;
            return;
        }
    }

    public override void ScanPlantedObstacle()
    {
        environmentEffect.ClearEffect();
        base.ScanPlantedObstacle();
        if (towerData.EffectTarget != EffectTarget.Towers) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedBuffTowerData.attackRange / 2, LayerMaskData.tower);

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

    protected override void OnDestroy()
    {
        if (towerData.EffectTarget != EffectTarget.Towers) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, adaptedBuffTowerData.attackRange / 2, LayerMaskData.tower);

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
