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
    [SerializeField] private int maxPoolSize = 60;

    // �̸� �� Ŭ�� ����
    private Dictionary<string, AudioClip> soundDict;

    // SFX�� AudioSource Ǯ
    private Queue<AudioSource> audioSourcePool;
    private int currentPoolCapacity;

    // BGM ���� AudioSource
    private AudioSource bgmPlayer;

    // SFX ���� ���� AudioSource
    private AudioSource loopSource;

    // ���� �� (0~1)
    private float sfxVolume;
    private float bgmVolume;
    private float masterVolume;

    // Ŭ�� ���� ���ε��� InputSystem
    private PointerInput pointerInput;
    private PointerInput.PointerActions pointerAction;

    private void Awake()
    {
        Init();

        // Ŭ�� ���� ���ε�
        pointerInput = new PointerInput();
        pointerAction = pointerInput.Pointer;
        pointerInput.Enable();
        pointerAction.Pressed.started += _ => PlaySFX("ClickSound");
    }

    private void Start()
    {
        // �⺻ �޴� BGM
        PlayBGM("PianoInstrumental1", loop: true);
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        // ����� ���� �ҷ�����
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 0.3f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 0.3f);
        masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 0.3f);

        // Ŭ�� ��ųʸ� �ʱ�ȭ
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
            soundDict[clip.name] = clip;

        // BGM ���� AudioSource ����
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.pitch = 1f;
        bgmPlayer.volume = bgmVolume * masterVolume;

        // SFX Ǯ ����
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var src = CreateNewAudioSource();
            src.enabled = false;
            audioSourcePool.Enqueue(src);
        }
        currentPoolCapacity = poolSize;

        // �ʱ� ���� ����
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    // AudioSource �⺻ ���� �Լ�
    private AudioSource CreateNewAudioSource()
    {
        var src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.pitch = 1f;
        src.volume = sfxVolume * masterVolume;
        return src;
    }

    // ����������������������������������������������������������������������������������������
    // SFX: �ܹ� ���
    // ����������������������������������������������������������������������������������������
    public void PlaySFX(string soundName)
    {
        if (!soundDict.TryGetValue(soundName, out var clip))
        {
            Debug.LogError($"[SoundManager] SFX clip not found: {soundName}");
            return;
        }

        AudioSource src;
        if (audioSourcePool.Count > 0)
        {
            src = audioSourcePool.Dequeue();
        }
        else if (currentPoolCapacity < maxPoolSize)
        {
            src = CreateNewAudioSource();
            currentPoolCapacity++;
        }
        else
        {
            Debug.LogWarning($"[SoundManager] Max pool ({maxPoolSize}) reached. Skipping {soundName}");
            return;
        }

        src.clip = clip;
        src.volume = sfxVolume * masterVolume;
        src.pitch = 1f;
        src.loop = false;
        src.enabled = true;
        src.Play();
        StartCoroutine(ReturnToPool(src, clip.length));
    }

    private IEnumerator ReturnToPool(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        src.Stop();
        src.enabled = false;
        src.loop = false;
        src.pitch = 1f;
        audioSourcePool.Enqueue(src);
    }

    // ����������������������������������������������������������������������������������������
    // SFX: ���� ��� ����
    // ����������������������������������������������������������������������������������������
    public void PlaySFXLoop(string soundName)
    {
        if (loopSource != null && loopSource.isPlaying) return;
        if (!soundDict.TryGetValue(soundName, out var clip)) return;

        if (loopSource == null)
        {
            loopSource = CreateNewAudioSource();
            loopSource.loop = true;
        }

        loopSource.clip = clip;
        loopSource.volume = sfxVolume * masterVolume;
        loopSource.pitch = 1f;
        loopSource.loop = true;
        loopSource.enabled = true;
        loopSource.Play();
    }

    // ����������������������������������������������������������������������������������������
    // SFX: ���� ��� ����
    // ����������������������������������������������������������������������������������������
    public void StopSFXLoop()
    {
        if (loopSource != null && loopSource.isPlaying)
        {
            loopSource.Stop();
            loopSource.enabled = false;
            loopSource.loop = false;
            loopSource.pitch = 1f;
        }
    }

    // ����������������������������������������������������������������������������������������
    // ���� StopSFX ȣ�� ȣȯ��
    // ����������������������������������������������������������������������������������������
    public void StopSFX(string soundName)
    {
        if (loopSource != null
            && loopSource.isPlaying
            && loopSource.clip.name == soundName)
        {
            StopSFXLoop();
        }
    }

    public void StopSFX()
    {
        StopSFXLoop();
    }

    // ����������������������������������������������������������������������������������������
    // BGM: ��� (loop=true �⺻)
    // ����������������������������������������������������������������������������������������
    public void PlayBGM(string bgmName, bool loop = true)
    {
        if (!soundDict.TryGetValue(bgmName, out var clip))
        {
            Debug.LogError($"[SoundManager] BGM clip not found: {bgmName}");
            return;
        }

        if (bgmPlayer.clip == clip
            && bgmPlayer.loop == loop
            && bgmPlayer.isPlaying)
            return;

        bgmPlayer.clip = clip;
        bgmPlayer.loop = loop;
        bgmPlayer.volume = bgmVolume * masterVolume;
        bgmPlayer.pitch = 1f;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();
    }

    // ����������������������������������������������������������������������������������������
    // BGM ��� ����/���� ��ȸ
    // ����������������������������������������������������������������������������������������
    public bool IsBgmPlaying() => bgmPlayer.isPlaying;

    public float GetBgmLength(string bgmName)
    {
        return soundDict.TryGetValue(bgmName, out var clip)
            ? clip.length
            : 0f;
    }

    // ����������������������������������������������������������������������������������������
    // ���� ����
    // ����������������������������������������������������������������������������������������
    public void SetSFXVolume(float vol)
    {
        sfxVolume = Mathf.Clamp01(vol);
        PlayerPrefs.SetFloat("SFX_VOLUME", sfxVolume);
        PlayerPrefs.Save();
        if (loopSource != null)
            loopSource.volume = sfxVolume * masterVolume;
    }

    public void SetBGMVolume(float vol)
    {
        bgmVolume = Mathf.Clamp01(vol);
        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.Save();
        bgmPlayer.volume = bgmVolume * masterVolume;
    }

    public void SetMasterVolume(float vol)
    {
        masterVolume = Mathf.Clamp01(vol);
        PlayerPrefs.SetFloat("MASTER_VOLUME", masterVolume);
        PlayerPrefs.Save();

        bgmPlayer.volume = bgmVolume * masterVolume;
        if (loopSource != null)
            loopSource.volume = sfxVolume * masterVolume;
    }
}
