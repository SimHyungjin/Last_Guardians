using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [System.Serializable]
    public class ProjectileEntry
    {
        public ProjectileType type;
        public ProjectileBase prefab; 
    }

    [SerializeField]
    private List<ProjectileEntry> projectileList;

    private Dictionary<ProjectileType, ProjectileBase> projectileMap;

    private void Awake()
    {
        projectileMap = new();

        foreach (var entry in projectileList)
        {
            if (!projectileMap.ContainsKey(entry.type))
                projectileMap.Add(entry.type, entry.prefab);
            else
                Debug.LogWarning($"[ProjectileFactory] 중복된 projectileType: {entry.type}");
        }
    }

    public void SpawnAndLaunch(Vector3 spawnPos,Vector2 targetPos,TowerData towerData)
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab))
        {
            Debug.LogError($"[ProjectileFactory] 타입에 해당하는 프리팹 없음: {towerData.ProjectileType}");
            return;
        }

        var projectile = PoolManager.Instance.Spawn(prefab);
        projectile.Launch(targetPos);

        // 이펙트생성하고 붙이기
        //AddEffectComponent(projectile.gameObject, towerData);
    }
    //이펙트 완성하면 붙이기
    //private void AddEffects(GameObject obj, TowerData towerData)
    //{
    //    switch (towerData.specialEffect)
    //    {
    //        case SpecialEffect.AttackPower:
    //            var atk = obj.AddComponent<AttackPowerEffect>();
    //            atk.Init(towerData.effectChance, towerData.effectValue, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.AttackSpeed:
    //            var spd = obj.AddComponent<AttackSpeedEffect>();
    //            spd.Init(towerData.effectChance, towerData.effectValue, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.BossDamage:
    //            var bossDmg = obj.AddComponent<BossDamageEffect>();
    //            bossDmg.Init(towerData.effectChance, towerData.effectValue);
    //            break;

    //        case SpecialEffect.BossDebuff:
    //            var bossDebuff = obj.AddComponent<BossDebuffEffect>();
    //            bossDebuff.Init(towerData.effectChance, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.Buff:
    //            var buff = obj.AddComponent<BuffEffect>();
    //            buff.Init(towerData.effectChance, towerData.effectValue, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.ChainAttack:
    //            var chain = obj.AddComponent<ChainAttackEffect>();
    //            chain.Init(towerData.effectChance, towerData.effectTargetCount, towerData.effectValue);
    //            break;

    //        case SpecialEffect.DefReduc:
    //            var def = obj.AddComponent<DefReducEffect>();
    //            def.Init(towerData.effectChance, towerData.effectValue, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.DotDamage:
    //            var dot = obj.AddComponent<DotDamageEffect>();
    //            dot.Init(towerData.effectChance, towerData.effectValue, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.Knockback:
    //            var knock = obj.AddComponent<KnockbackEffect>();
    //            knock.Init(towerData.effectChance, towerData.effectValue);
    //            break;

    //        case SpecialEffect.MultyTarget:
    //            var multi = obj.AddComponent<MultyTargetEffect>();
    //            multi.Init(towerData.effectTargetCount);
    //            break;

    //        case SpecialEffect.Slow:
    //            var slow = obj.AddComponent<SlowEffect>();
    //            slow.Init(towerData.effectChance, towerData.effectValue, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.Stun:
    //            var stun = obj.AddComponent<StunEffect>();
    //            stun.Init(towerData.effectChance, towerData.effectDuration);
    //            break;

    //        case SpecialEffect.Summon:
    //            var summon = obj.AddComponent<SummonEffect>();
    //            summon.Init(towerData.effectValue, towerData.effectTargetCount); // 필요시 조정
    //            break;

    //        case SpecialEffect.None:
    //        default:
    //            break;
    //    }
}

