using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSceneSwitcher : MonoBehaviour
{
    [Header("Start/Main Scene BGM")]
    [SerializeField] private string menuBgm = "PianoInstrumental1";

    [Header("Game Scene BGM 리스트 (순차 재생)")]
    [SerializeField] private string[] gameSceneBgms;

    private Coroutine gameBgmLoop;

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
        // 메뉴 씬: 단일 BGM(loop=true)
        if (next.name == "StartScene" || next.name == "MainScene")
        {
            StopGameBgmLoop();
            SoundManager.Instance.PlayBGM(menuBgm, loop: true);
        }
        // 게임 씬: 순차 재생 시작
        else if (next.name == "GameScene")
        {
            StopGameBgmLoop();
            gameBgmLoop = StartCoroutine(PlayGameSceneBgms());
        }
        else
        {
            StopGameBgmLoop();
        }
    }

    private IEnumerator PlayGameSceneBgms()
    {
        int idx = 0;
        while (true)
        {
            // 한 번만 재생(loop=false)
            SoundManager.Instance.PlayBGM(gameSceneBgms[idx], loop: false);

            // 재생이 완전히 끝날 때까지 대기
            yield return new WaitUntil(() => !SoundManager.Instance.IsBgmPlaying());

            idx = (idx + 1) % gameSceneBgms.Length;
        }
    }

    private void StopGameBgmLoop()
    {
        if (gameBgmLoop != null)
        {
            StopCoroutine(gameBgmLoop);
            gameBgmLoop = null;
        }
    }
}
