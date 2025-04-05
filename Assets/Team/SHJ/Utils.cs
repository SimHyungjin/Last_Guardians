using UnityEngine;

public class Utils
{
    public static T InstantiateResource<T>(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"리소시스 패스 확인 필요 : {path}");
            return default;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

        if (go.TryGetComponent<T>(out T component))
        {
            return component;
        }
        else
        {
            return default;
        }
    }
    public static GameObject InstantiateResource(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"리소시스 패스 확인 필요: {path}");
            return null;
        }
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }
}
