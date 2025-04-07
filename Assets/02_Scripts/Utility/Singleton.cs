using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<T>();

            if (instance != null) return instance;

            var obj = new GameObject(typeof(T).Name);
            instance = obj.AddComponent<T>();
            //DontDestroyOnLoad(obj);

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
