using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SceneLoader
{
    public static void LoadScene(string sceneName, Action beforeSceneLoad = null, Action afterSceneLoad = null)
    {
        beforeSceneLoad?.Invoke();

        var go = new GameObject("SceneLoaderTemp");
        GameObject.DontDestroyOnLoad(go);
        var loader = go.AddComponent<SceneLoaderRunner>();
        loader.StartLoading(sceneName, afterSceneLoad);
    }

    public static void LoadSceneWithFade(string sceneName, bool selectFadeOut, Action beforeSceneLoad = null, Action afterSceneLoad = null)
    {
        beforeSceneLoad?.Invoke();

        var go = new GameObject("SceneLoaderTemp");
        GameObject.DontDestroyOnLoad(go);
        var loader = go.AddComponent<SceneLoaderRunner>();
        loader.StartLoadingWithFade(sceneName, selectFadeOut, afterSceneLoad);
    }

    private class SceneLoaderRunner : MonoBehaviour
    {
        public void StartLoading(string sceneName, Action afterSceneLoad = null)
        {
            StartCoroutine(LoadSceneRoutine(sceneName, afterSceneLoad));
        }

        private IEnumerator LoadSceneRoutine(string sceneName, Action afterSceneLoad)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone) yield return null;
            afterSceneLoad?.Invoke();
            Destroy(gameObject);
        }

        public void StartLoadingWithFade(string sceneName, bool selectFadeOut, Action afterSceneLoad = null)
        {
            var fadeCanvas = new GameObject("FadeCanvas");
            fadeCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var group = fadeCanvas.AddComponent<CanvasGroup>();
            fadeCanvas.AddComponent<GraphicRaycaster>();
            var image = new GameObject("FadeImage").AddComponent<Image>();
            image.transform.SetParent(fadeCanvas.transform, false);
            image.rectTransform.anchorMin = Vector2.zero;
            image.rectTransform.anchorMax = Vector2.one;
            image.rectTransform.offsetMin = Vector2.zero;
            image.rectTransform.offsetMax = Vector2.zero;
            image.color = Color.black;

            var fadeEffect = fadeCanvas.AddComponent<SceneFadeEffect>();
            fadeEffect.canvasGroup = group;

            StartCoroutine(LoadSceneWithFade(sceneName, selectFadeOut, fadeEffect, afterSceneLoad));
        }

        private IEnumerator LoadSceneWithFade(string sceneName, bool selectFadeOut, SceneFadeEffect fadeEffect, Action afterSceneLoad)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone) yield return null;

            afterSceneLoad?.Invoke();
            if(selectFadeOut) yield return fadeEffect.FadeOut();
            else yield return fadeEffect.FadeIn();

            Destroy(fadeEffect.gameObject);
            Destroy(gameObject);
        }
    }

    private class SceneFadeEffect : MonoBehaviour
    {
        public float fadeDuration = 1f;
        public CanvasGroup canvasGroup;

        public IEnumerator FadeIn(Action onComplete = null)
        {
            yield return Fade(1f, 0f);
            onComplete?.Invoke();
        }

        public IEnumerator FadeOut(Action onComplete = null)
        {
            yield return Fade(0f, 1f);
            onComplete?.Invoke();
        }

        private IEnumerator Fade(float startAlpha, float endAlpha)
        {
            float time = 0f;
            canvasGroup.alpha = startAlpha;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = endAlpha;
        }
    }
}
