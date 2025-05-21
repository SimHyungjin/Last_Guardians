using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IEffect
{
    void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect);
    void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect);
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
        { SpecialEffect.ChainAttack, typeof(ProjectileChainAttackEffect) },
        { SpecialEffect.Stun, typeof(ProjectileStunEffect) },
        { SpecialEffect.BossDamage, typeof(ProjectileBossDamageEffect) },
        { SpecialEffect.BossDebuff, typeof(ProjectileBossDebuffEffect) },
        { SpecialEffect.DefReduc, typeof(ProjectileDefReducEffect) },
        { SpecialEffect.Critical, typeof(ProjectileCriticalEffect)},
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
        }
    }
    /// <summary>
    /// 다중 발사체 발사, 각 발사체가 서로 다른 각도로 발사됨
    /// 각각의 타워에 맞는 프로젝타일을 생성하여 발사
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetPos"></param>
    /// <param name="towerData"></param>
    /// <param name="adaptedTowerData"></param>
    /// <param name="parent"></param>
    /// <param name="buffTowerIndex"></param>
    /// <param name="shotCount"></param>
    /// <param name="environmentEffect"></param>
    public void MultiSpawnAndLaunch<T>(Vector2 targetPos, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, Transform parent, List<int> buffTowerIndex, int shotCount, EnvironmentEffect environmentEffect) where T : ProjectileBase
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var prefab)) return;

        var castedPrefab = prefab as T;
        if (castedPrefab == null) return;

        Vector2 origin = parent.position;
        float originDistance = Vector2.Distance(origin, targetPos);
        Vector2 baseDir = (targetPos - origin).normalized;

        float maxAngle = 20f;
        int half = shotCount / 2;
        for (int i = 0; i < shotCount; i++)
        {
            int index = i - half;
            if (shotCount % 2 == 0 && index >= 0) index += 1;

            float angle = (shotCount == 1) ? 0f : (maxAngle / (shotCount - 1)) * index;
            Vector2 rotatedDir = Quaternion.Euler(0, 0, angle) * baseDir;
            Vector2 launchPos = origin + rotatedDir * 0.5f;

            var projectile = PoolManager.Instance.Spawn(castedPrefab, parent);
            projectile.transform.position = launchPos;
            if (shotCount == 1)
            {
                projectile.transform.rotation = Quaternion.identity;
            }
            else
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, rotatedDir));
            }
            projectile.Init(towerData, adaptedTowerData, buffTowerIndex, environmentEffect);
            AddAllEffects(projectile, buffTowerIndex);
            projectile.Launch(origin + rotatedDir * originDistance);
        }
    }

    /// <summary>
    /// 프리팹을 반환하는 메서드, 체인샷에서 사용됨
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="towerData"></param>
    /// <returns></returns>
    public T ReturnPrefabs<T>(TowerData towerData) where T : ProjectileBase
    {
        if (!projectileMap.TryGetValue(towerData.ProjectileType, out var basePrefab))
        {
            return null;
        }

        if (basePrefab is T casted)
        {
            return casted;
        }
        return null;
    }

    /// <summary>
    /// 타워에서 저장된 이펙트 리스트를 가져와 프로젝타일에 실제 이펙트를 저장
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="buffTowerIndex"></param>
    public void AddAllEffects(ProjectileBase projectile, List<int> buffTowerIndex)
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
            if (buffTowerData != null && (buffTowerData.SpecialEffect != SpecialEffect.None))
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

