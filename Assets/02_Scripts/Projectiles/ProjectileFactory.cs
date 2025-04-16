using System;
using System.Collections.Generic;
using UnityEngine;
public interface IEffect
{
    void Apply(BaseMonster target, TowerData towerData);
    void Apply(BaseMonster target, TowerData towerData, float chance);
}



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

    private static readonly Dictionary<SpecialEffect, Type> effectTypeMap = new()
    {
        { SpecialEffect.DotDamage, typeof(ProjectileDotDamageEffect) },
        { SpecialEffect.Slow, typeof(ProjectileSlowEffect) },
        { SpecialEffect.MultyTarget, typeof(ProjectileMultyTargetEffect) },//미구현
        { SpecialEffect.ChainAttack, typeof(ProjectileChainAttackEffect) },//미구현
        { SpecialEffect.Stun, typeof(ProjectileStunEffect) },
        { SpecialEffect.BossDamage, typeof(ProjectileBossDamageEffect) },//미구현
        { SpecialEffect.BossDebuff, typeof(ProjectileBossDebuffEffect) },//미구현
        { SpecialEffect.DefReduc, typeof(ProjectileDefReducEffect) },//미구현
        { SpecialEffect.Knockback, typeof(ProjectileKnockbackEffect) },//미구현
        { SpecialEffect.Buff, typeof(ProjectileBuffEffect) },//미구현
        { SpecialEffect.AttackPower, typeof(ProjectileAttackPowerEffect) },//미구현
        { SpecialEffect.AttackSpeed, typeof(ProjectileAttackSpeedEffect) },//미구현
        { SpecialEffect.Summon, typeof(ProjectileSummonEffect) },//미구현
    };

    private void Awake()
    {
        projectileMap = new();

        foreach (var entry in projectileList)
        {
            if (!projectileMap.ContainsKey(entry.type))
            {
                projectileMap.Add(entry.type, entry.prefab);
            }
            else
                Debug.LogWarning($"[ProjectileFactory] 중복된 projectileType: {entry.type}");
        }
    }

    public void SpawnAndLaunch(Vector2 targetPos, TowerData towerData, Transform parent)
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab))
        {
            Debug.LogError($"[ProjectileFactory] 타입에 해당하는 프리팹 없음: {towerData.ProjectileType}");
            return;
        }
        var projectile = PoolManager.Instance.Spawn(prefab, parent);
        Debug.Log($"[ProjectileFactory] {towerData.ProjectileType} 발사 위치: {targetPos}");
        projectile.Init(towerData);
        projectile.Launch(targetPos);
        AddEffectComponent(projectile, towerData);
    }
    private void AddEffectComponent(ProjectileBase projectile, TowerData data)
    {
        if (data.SpecialEffect == SpecialEffect.None) return ;

        if (effectTypeMap.TryGetValue(data.SpecialEffect, out var effectType))
        {
            var go = projectile.gameObject;

            // 중복 방지
            if (!go.TryGetComponent(effectType, out var existing))
            {
                var added = go.AddComponent(effectType) as IEffect;
                projectile.effect = added;
            }
            else
            {
                projectile.effect = existing as IEffect;
            }
        }
    }


}

