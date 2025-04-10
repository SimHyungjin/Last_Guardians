using System.Linq;
using UnityEngine;

public class Utils
{
    /// <summary>
    /// Resources 경로에서 프리팹을 로드한 후, 인스턴스화하여 지정한 컴포넌트를 반환합니다.
    /// </summary>
    /// <typeparam name="T">반환할 컴포넌트 타입</typeparam>
    /// <param name="path">Resources 내부의 경로</param>
    /// <param name="parent">인스턴스화할 부모 Transform (선택)</param>
    /// <returns>프리팹에서 추출한 컴포넌트, 없으면 default</returns>
    public static T InstantiateComponentFromResource<T>(string path, Transform parent = null)
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
    /// <summary>
    /// Resources 경로에서 프리팹을 로드한 후, GameObject 형태로 인스턴스화합니다.
    /// </summary>
    /// <param name="path">Resources 내부의 경로</param>
    /// <param name="parent">인스턴스화할 부모 Transform (선택)</param>
    /// <returns>생성된 GameObject, 실패 시 null</returns>
    public static GameObject InstantiatePrefabFromResource(string path, Transform parent = null)
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
