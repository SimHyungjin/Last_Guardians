using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;

    [Header("Object Pool Settings")]
    [SerializeField] private int poolSize = 30;

    private Dictionary<string, AudioClip> soundDict;
    private Queue<AudioSource> audioSourcePool;
    private AudioSource bgmPlayer;

    // ���� ��� ���� AudioSource
    private AudioSource loopSource;

    // ���� ��(0~1)
    private float sfxVolume;
    private float bgmVolume;
    private float masterVolume;

    // �Է� �ý��� (Ŭ�� �����)
    private PointerInput pointerInput;
    private PointerInput.PointerActions pointerAction;

    private void Awake()
    {
        Init();

        // InputSystem���� Ŭ�� ���� ���ε�
        pointerInput = new PointerInput();
        pointerAction = pointerInput.Pointer;
        pointerInput.Enable();
        pointerAction.Pressed.started += _ => PlaySFX("ClickSound");
    }

    private void Start()
    {
        PlayBGM("beanfeast");
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        // ���� �ҷ�����
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 0.3f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 0.3f);
        masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 0.3f);

        // Ŭ�� ��ųʸ�
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
            soundDict[clip.name] = clip;

        // BGM ������ҽ�
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.playOnAwake = false;

        // SFX Ǯ
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.enabled = false;
            audioSourcePool.Enqueue(src);
        }

        // �ʱ� ���� �ݿ�
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    // 1ȸ ��� SFX
    public void PlaySFX(string soundName)
    {
        if (!soundDict.TryGetValue(soundName, out var clip))
        {
            return;
        }

        AudioSource src = audioSourcePool.Count > 0
            ? audioSourcePool.Dequeue()
            : gameObject.AddComponent<AudioSource>();

        src.clip = clip;
        src.volume = sfxVolume * masterVolume;
        src.loop = false;
        src.enabled = true;
        src.Play();
        StartCoroutine(ReturnToPool(src, clip.length));
    }

    // ���� ��� SFX ����
    public void PlaySFXLoop(string soundName)
    {
        if (loopSource != null && loopSource.isPlaying) return;
        if (!soundDict.TryGetValue(soundName, out var clip)) return;

        if (loopSource == null)
        {
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.playOnAwake = false;
            loopSource.loop = true;
        }

        loopSource.clip = clip;
        loopSource.volume = sfxVolume * masterVolume;
        loopSource.Play();
    }

    // ���� ��� SFX ����
    public void StopSFXLoop()
    {
        if (loopSource != null && loopSource.isPlaying)
            loopSource.Stop();
    }

    private IEnumerator ReturnToPool(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        src.enabled = false;
        audioSourcePool.Enqueue(src);
    }

    // BGM
    public void PlayBGM(string bgmName)
    {
        if (!soundDict.TryGetValue(bgmName, out var clip)) return;
        if (bgmPlayer.clip == clip) return;
        bgmPlayer.clip = clip;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume * masterVolume;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying) bgmPlayer.Stop();
    }

    // ���� ����
    public void SetSFXVolume(float vol)
    {
        sfxVolume = Mathf.Clamp01(vol);
        PlayerPrefs.SetFloat("SFX_VOLUME", sfxVolume);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float vol)
    {
        bgmVolume = Mathf.Clamp01(vol);
        bgmPlayer.volume = bgmVolume * masterVolume;
        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float vol)
    {
        masterVolume = Mathf.Clamp01(vol);
        bgmPlayer.volume = bgmVolume * masterVolume;
        PlayerPrefs.SetFloat("MASTER_VOLUME", masterVolume);
        PlayerPrefs.Save();
    }
}
