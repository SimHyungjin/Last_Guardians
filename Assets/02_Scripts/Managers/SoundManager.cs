using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;

    [Header("Object Pool Settings")]
    [SerializeField] private int poolSize = 30;

    private Dictionary<string, AudioClip> soundDict;
    private Queue<AudioSource> audioSourcePool;
    private AudioSource bgmPlayer;

    private float sfxVolume = 0.3f;     // ����� ���� SFX ����
    private float bgmVolume = 0.3f;     // ����� ���� BGM ����
    private float masterVolume = 0.3f;  // ��ü ����

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        PlayBGM("beanfeast");
    }


    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        // ���� ��ųʸ� �ʱ�ȭ
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
            soundDict[clip.name] = clip;

        // BGM �÷��̾� �ʱ�ȭ
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.playOnAwake = false;

        // SFX ������ҽ� Ǯ �ʱ�ȭ
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.enabled = false;
            audioSourcePool.Enqueue(src);
        }
    }

    // SFX ���
    public void PlaySFX(string soundName)
    {
        if (!soundDict.TryGetValue(soundName, out var clip)) return;

        AudioSource src;
        if (audioSourcePool.Count > 0)
            src = audioSourcePool.Dequeue();
        else
            src = gameObject.AddComponent<AudioSource>();

        src.clip = clip;
        src.volume = sfxVolume * masterVolume;  // �� masterVolume ���ϱ�
        src.enabled = true;
        src.Play();

        StartCoroutine(ReturnToPool(src, clip.length));
    }

    private IEnumerator ReturnToPool(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        src.enabled = false;
        audioSourcePool.Enqueue(src);
    }

    // BGM ���
    public void PlayBGM(string bgmName)
    {
        if (!soundDict.TryGetValue(bgmName, out var clip)) return;
        if (bgmPlayer.clip == clip) return;

        bgmPlayer.clip = clip;
        bgmPlayer.volume = bgmVolume * masterVolume;  // �� masterVolume ���ϱ�
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();
    }

    // SFX ���� ���� (0~1)
    public void SetSFXVolume(float vol)
    {
        sfxVolume = Mathf.Clamp01(vol);
    }

    // BGM ���� ���� (0~1)
    public void SetBGMVolume(float vol)
    {
        bgmVolume = Mathf.Clamp01(vol);
        bgmPlayer.volume = bgmVolume * masterVolume;  // �� ��� �ݿ�
    }

    // ��ü(������) ���� ���� (0~1)
    public void SetMasterVolume(float vol)
    {
        masterVolume = Mathf.Clamp01(vol);
        // ���� BGM ���� ��� ������Ʈ
        bgmPlayer.volume = bgmVolume * masterVolume;
    }
}
