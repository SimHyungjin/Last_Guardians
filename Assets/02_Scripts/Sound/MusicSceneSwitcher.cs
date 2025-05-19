using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSceneSwitcher : MonoBehaviour
{
    [Header("Start/Main Scene BGM")]
    [SerializeField] private string menuBgm = "PianoInstrumental1";

    [Header("Game Scene BGM ����Ʈ (���� ���)")]
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
        // �޴� ��: ���� BGM(loop=true)
        if (next.name == "StartScene" || next.name == "MainScene")
        {
            StopGameBgmLoop();
            SoundManager.Instance.PlayBGM(menuBgm, loop: true);
        }
        // ���� ��: ���� ��� ����
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
            // �� ���� ���(loop=false)
            SoundManager.Instance.PlayBGM(gameSceneBgms[idx], loop: false);

            // ����� ������ ���� ������ ���
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
