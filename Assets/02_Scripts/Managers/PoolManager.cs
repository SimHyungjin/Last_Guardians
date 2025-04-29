using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 풀링된 오브젝트가 활성화되거나 반환될 때 추가 처리가 필요하다면 이 인터페이스를 상속하여 구현해 주세요.
/// </summary>
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<System.Type, Queue<Component>> pools = new();
    private Dictionary<System.Type, Transform> poolParents = new();

    private Dictionary<string, Queue<Component>> poolNames = new();
    private Dictionary<string, Transform> poolPa = new();

    public T Spawn<T>(T prefab, Transform parent = null) where T : Component
    {
        System.Type type = typeof(T);
        if (!pools.ContainsKey(type))
        {
            pools[type] = new Queue<Component>();

            GameObject poolParentObj = new GameObject(prefab.name + "Pool");
            poolParentObj.transform.SetParent(this.transform);
            poolParents[type] = poolParentObj.transform;
        }

        T obj;

        if (pools[type].Count > 0)
        {
            obj = pools[type].Dequeue() as T;
            obj.transform.position = parent != null ? parent.transform.position : obj.transform.position;
        }
        else
        {
            Transform spawnParent = parent != null ? parent : poolParents[type];
            obj = Instantiate(prefab, spawnParent.position,spawnParent.rotation, parent);
        }

        obj.gameObject.SetActive(true);

        if (obj is IPoolable poolable)
            poolable.OnSpawn();

        return obj;
    }

    public T SpawnbyPrefabName<T>(T prefab, Transform parent = null) where T : Component
    {
        string key = prefab.gameObject.name;  // 프리팹 이름 기준으로 관리

        if (!poolNames.ContainsKey(key))
        {
            poolNames[key] = new Queue<Component>();
            GameObject poolParentObj = new GameObject(prefab.name + "Pool");
            poolParentObj.transform.SetParent(this.transform);
            poolPa[key] = poolParentObj.transform;
        }

        T obj;

        if (poolNames[key].Count > 0)
        {
            obj = poolNames[key].Dequeue() as T;
            obj.transform.position = parent != null ? parent.transform.position : obj.transform.position;
        }
        else
        {
            Transform spawnParent = parent != null ? parent : poolPa[key];
            obj = Instantiate(prefab, spawnParent.position, spawnParent.rotation, parent);
        }

        obj.gameObject.SetActive(true);

        if (obj is IPoolable poolable)
            poolable.OnSpawn();

        return obj;
    }

    public void Despawn<T>(T obj) where T : Component
    {
        System.Type type = typeof(T);

        if (obj is IPoolable poolable)
            poolable.OnDespawn();

        obj.gameObject.SetActive(false);

        if (!pools.ContainsKey(type))
        {
            pools[type] = new Queue<Component>();
            GameObject poolParentObj = new GameObject(typeof(T).Name + "Pool");
            poolParentObj.transform.SetParent(this.transform);
            poolParents[type] = poolParentObj.transform;
        }

        obj.transform.SetParent(poolParents[type]);

        pools[type].Enqueue(obj);
    }

    public void DespawnbyPrefabName<T>(T obj) where T : Component
    {
        string key = obj.gameObject.name.Replace("(Clone)", "").Trim(); // 프리팹 기준으로

        if (obj is IPoolable poolable)
            poolable.OnDespawn();

        obj.gameObject.SetActive(false);

        if (!poolNames.ContainsKey(key))
        {
            poolNames[key] = new Queue<Component>();
            GameObject poolParentObj = new GameObject(key + "Pool");
            poolParentObj.transform.SetParent(this.transform);
            poolPa[key] = poolParentObj.transform;
        }

        obj.transform.SetParent(poolPa[key]);
        poolNames[key].Enqueue(obj);
    }

    public void ClearPool()
    {
        pools.Clear();
        poolParents.Clear();
    }
}