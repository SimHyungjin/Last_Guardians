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
        { SpecialEffect.MultyTarget, typeof(ProjectileMultyTargetEffect) },//미구현
        { SpecialEffect.ChainAttack, typeof(ProjectileChainAttackEffect) },//미구현//반쯤구현 ->멀티타겟이펙트 가져오면해결
        { SpecialEffect.Stun, typeof(ProjectileStunEffect) },
        { SpecialEffect.BossDamage, typeof(ProjectileBossDamageEffect) },
        { SpecialEffect.BossDebuff, typeof(ProjectileBossDebuffEffect) },
        { SpecialEffect.DefReduc, typeof(ProjectileDefReducEffect) },
        { SpecialEffect.Knockback, typeof(ProjectileKnockbackEffect) },
        { SpecialEffect.Buff, typeof(ProjectileBuffEffect) },//미구현
        { SpecialEffect.AttackPower, typeof(ProjectileAttackPowerEffect) },
        { SpecialEffect.Summon, typeof(ProjectileSummonEffect) },//미구현
        { SpecialEffect.Silence, typeof(ProjectileSilenceEffect) },
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
                Debug.LogWarning($"[ProjectileFactory] 중복된 projectileType: {entry.type}");
        }
    }
    public void SpawnAndLaunch<T>(Vector2 targetPos, TowerData towerData,AdaptedTowerData adaptedTowerData ,Transform parent,List<int> buffTowerIndex) where T : ProjectileBase
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab))
        {
            Debug.LogError($"[ProjectileFactory] 타입에 해당하는 프리팹 없음: {towerData.ProjectileType}");
            return;
        }

        var castedPrefab = prefab as T;
        if (castedPrefab == null)
        {
            Debug.LogError($"[ProjectileFactory] 프리팹 타입 불일치: {towerData.ProjectileType} → {typeof(T)} 기대됨");
            return;
        }

        var projectile = PoolManager.Instance.Spawn(castedPrefab, parent);
        projectile.Init(towerData, buffTowerIndex);
        AddAllEffects(projectile, towerData, buffTowerIndex);
        projectile.Launch(targetPos); // 이펙트 추가
    }
    public void MultiSpawnAndLaunch<T>(Vector2 targetPos, TowerData towerData, AdaptedTowerData adaptedTowerData ,Transform parent, List<int> buffTowerIndex,int shotCount) where T : ProjectileBase
    {
        
            if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab)) return;

            var castedPrefab = prefab as T;
            if (castedPrefab == null) return;

            Vector2 origin = parent.position;
            Vector2 baseDir = (targetPos - origin).normalized;
            float maxAngle = 45f;
            int maxShots = Mathf.Min(shotCount, 7);

            int half = maxShots / 2;
            for (int i = 0; i < maxShots; i++)
            {
                int index = i - half;
                if (maxShots % 2 == 0 && index >= 0) index += 1;

                float angle = (maxAngle / (maxShots - 1)) * index;
                Vector2 rotatedDir = Quaternion.Euler(0, 0, angle) * baseDir;
                Vector2 launchPos = origin + rotatedDir * 0.5f;

                var projectile = PoolManager.Instance.Spawn(castedPrefab, parent);
                projectile.transform.position = launchPos;
                projectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, rotatedDir));
                projectile.Init(towerData, buffTowerIndex);
                AddAllEffects(projectile, towerData, buffTowerIndex);
                projectile.Launch(origin + rotatedDir * 10f);
            }
        }

    public T ReturnPrefabs<T>(TowerData towerData) where T : ProjectileBase
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var basePrefab))
        {
            Debug.LogError($"[ProjectileFactory] 타입에 맞는 프리팹 없음: {towerData.ProjectileType}");
            return null;
        }

        if (basePrefab is T casted)
        {
            return casted;
        }

        Debug.LogError($"[ProjectileFactory] 프리팹 타입이 기대한 {typeof(T)}이 아님: 실제는 {basePrefab.GetType()}");
        return null;
    }

    private void AddAllEffects(ProjectileBase projectile, TowerData baseData,List<int> buffTowerIndex)
    {
        var go = projectile.gameObject;
        var effectList = new List<TowerData>();

        var components = go.GetComponents<MonoBehaviour>();
        foreach (var comp in components)
        {
            if (comp is IEffect)
            {
                Destroy(comp);
            }
        }

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

