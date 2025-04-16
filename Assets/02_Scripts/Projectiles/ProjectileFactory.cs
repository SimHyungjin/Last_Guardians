using System;
using System.Collections.Generic;
using System.Linq;
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
        { SpecialEffect.None, null }
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
    public void SpawnAndLaunch<T>(Vector2 targetPos, TowerData towerData, Transform parent,List<int> buffTowerIndex) where T : ProjectileBase
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab))
        {
            Debug.LogError($"[ProjectileFactory] Ÿ�Կ� �ش��ϴ� ������ ����: {towerData.ProjectileType}");
            return;
        }

        var castedPrefab = prefab as T;
        if (castedPrefab == null)
        {
            Debug.LogError($"[ProjectileFactory] ������ Ÿ�� ����ġ: {towerData.ProjectileType} �� {typeof(T)} ����");
            return;
        }

        var projectile = PoolManager.Instance.Spawn(castedPrefab, parent);
        projectile.Init(towerData);
        projectile.Launch(targetPos);
        AddAllEffects(projectile, towerData, buffTowerIndex); // ����Ʈ �߰�
        //AddEffectComponent(projectile, towerData);
    }
    //private void AddEffectComponent(ProjectileBase projectile, TowerData data)
    //{
    //    if (data.SpecialEffect == SpecialEffect.None) return ;

    //    if (effectTypeMap.TryGetValue(data.SpecialEffect, out var effectType))
    //    {
    //        var go = projectile.gameObject;

    //        // �ߺ� ����
    //        if (!go.TryGetComponent(effectType, out var existing))
    //        {
    //            var added = go.AddComponent(effectType) as IEffect;
    //            projectile.effect = added;
    //        }
    //        else
    //        {
    //            projectile.effect = existing as IEffect;
    //        }
    //    }
    //}
    private void AddAllEffects(ProjectileBase projectile, TowerData baseData,List<int> buffTowerIndex)
    {
        var go = projectile.gameObject;
        var effectList = new List<TowerData>();

        foreach (int index in buffTowerIndex)
        {
            var buffTowerData = TowerManager.Instance.GetTowerData(index);
            if (buffTowerData != null && buffTowerData.SpecialEffect != SpecialEffect.None)
            {
                effectList.Add(buffTowerData);
            }
        }

        Dictionary<Type, IEffect> finalEffects = new();

        foreach (var data in effectList)
        {
            if (!effectTypeMap.TryGetValue(data.SpecialEffect, out var effectType)) continue;
                var effect = go.AddComponent(effectType) as IEffect;
                finalEffects.Add(effectType, effect);

        }
        projectile.effects?.Clear();
        projectile.effects = finalEffects.Values.Where(e => e != null).ToList();
    }
}

