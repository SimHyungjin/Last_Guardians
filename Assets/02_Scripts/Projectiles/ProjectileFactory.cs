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
        { SpecialEffect.MultyTarget, typeof(ProjectileMultyTargetEffect) },//�̱���
        { SpecialEffect.ChainAttack, typeof(ProjectileChainAttackEffect) },//�̱���
        { SpecialEffect.Stun, typeof(ProjectileStunEffect) },
        { SpecialEffect.BossDamage, typeof(ProjectileBossDamageEffect) },//�̱���
        { SpecialEffect.BossDebuff, typeof(ProjectileBossDebuffEffect) },//�̱���
        { SpecialEffect.DefReduc, typeof(ProjectileDefReducEffect) },//�̱���
        { SpecialEffect.Knockback, typeof(ProjectileKnockbackEffect) },//�̱���
        { SpecialEffect.Buff, typeof(ProjectileBuffEffect) },//�̱���
        { SpecialEffect.AttackPower, typeof(ProjectileAttackPowerEffect) },//�̱���
        { SpecialEffect.AttackSpeed, typeof(ProjectileAttackSpeedEffect) },//�̱���
        { SpecialEffect.Summon, typeof(ProjectileSummonEffect) },//�̱���
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
                Debug.LogWarning($"[ProjectileFactory] �ߺ��� projectileType: {entry.type}");
        }
    }

    public void SpawnAndLaunch(Vector2 targetPos, TowerData towerData, Transform parent)
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab))
        {
            Debug.LogError($"[ProjectileFactory] Ÿ�Կ� �ش��ϴ� ������ ����: {towerData.ProjectileType}");
            return;
        }
        var projectile = PoolManager.Instance.Spawn(prefab, parent);
        Debug.Log($"[ProjectileFactory] {towerData.ProjectileType} �߻� ��ġ: {targetPos}");
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

            // �ߺ� ����
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

