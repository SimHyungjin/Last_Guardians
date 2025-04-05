using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<System.Type, Queue<Component>> pools = new();

    public T Spawn<T>(T prefab, Transform parent = null) where T : Component
    {
        System.Type type = typeof(T);

        if (!pools.ContainsKey(type))
        {
            pools[type] = new Queue<Component>();
        }

        T obj;

        if (pools[type].Count > 0)
        {
            obj = pools[type].Dequeue() as T;
        }
        else
        {
            obj = Instantiate(prefab, parent != null ? parent : transform);
        }
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Despawn<T>(T obj) where T : Component
    {
        System.Type type = typeof(T);
        obj.gameObject.SetActive(false);

        if (!pools.ContainsKey(type))
            pools[type] = new Queue<Component>();

        pools[type].Enqueue(obj);
    }

    public void ClearPool()
    {
        pools.Clear();
    }
}