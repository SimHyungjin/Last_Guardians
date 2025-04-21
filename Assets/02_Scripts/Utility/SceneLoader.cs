using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(string sceneName, Action beforeSceneLoad = null, Action afterSceneLoad = null)
    {
        beforeSceneLoad?.Invoke();

        var go = new GameObject("SceneLoaderTemp");
        GameObject.DontDestroyOnLoad(go);
        var loader = go.AddComponent<SceneLoaderRunner>();
        loader.StartLoading(sceneName);
    }

    private class SceneLoaderRunner : MonoBehaviour
    {
        public void StartLoading(string sceneName, Action afterSceneLoad = null)
        {
            StartCoroutine(LoadSceneRoutine(sceneName, afterSceneLoad));
        }

        private IEnumerator LoadSceneRoutine(string sceneName, Action afterSceneLoad = null)
        {

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone) yield return null;
            afterSceneLoad?.Invoke();
            Destroy(gameObject);
        }
    }
}
