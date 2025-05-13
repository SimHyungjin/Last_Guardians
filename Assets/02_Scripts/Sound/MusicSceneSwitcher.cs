using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSceneSwitcher : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene prev, Scene next)
    {
        Debug.Log($"Active changed: {prev.name} ¡æ {next.name}");
        if (next.name == "StartScene")
            SoundManager.Instance.PlayBGM("beanfeast");
        if (next.name == "MainScene")
            SoundManager.Instance.PlayBGM("beanfeast");
        else if (next.name == "GameScene")
            SoundManager.Instance.PlayBGM("drama");
    }
}
