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

    private float sfxVolume = 0.3f;     // 사용자 설정 SFX 볼륨
    private float bgmVolume = 0.3f;     // 사용자 설정 BGM 볼륨
    private float masterVolume = 0.3f;  // 전체 볼륨

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

        // 사운드 딕셔너리 초기화
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
            soundDict[clip.name] = clip;

        // BGM 플레이어 초기화
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.playOnAwake = false;

        // SFX 오디오소스 풀 초기화
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.enabled = false;
            audioSourcePool.Enqueue(src);
        }
    }

    // SFX 재생
    public void PlaySFX(string soundName)
    {
        if (!soundDict.TryGetValue(soundName, out var clip)) return;

        AudioSource src;
        if (audioSourcePool.Count > 0)
            src = audioSourcePool.Dequeue();
        else
            src = gameObject.AddComponent<AudioSource>();

        src.clip = clip;
        src.volume = sfxVolume * masterVolume;  // ← masterVolume 곱하기
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

    // BGM 재생
    public void PlayBGM(string bgmName)
    {
        if (!soundDict.TryGetValue(bgmName, out var clip)) return;
        if (bgmPlayer.clip == clip) return;

        bgmPlayer.clip = clip;
        bgmPlayer.volume = bgmVolume * masterVolume;  // ← masterVolume 곱하기
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();
    }

    // SFX 볼륨 설정 (0~1)
    public void SetSFXVolume(float vol)
    {
        sfxVolume = Mathf.Clamp01(vol);
    }

    // BGM 볼륨 설정 (0~1)
    public void SetBGMVolume(float vol)
    {
        bgmVolume = Mathf.Clamp01(vol);
        bgmPlayer.volume = bgmVolume * masterVolume;  // ← 즉시 반영
    }

    // 전체(마스터) 볼륨 설정 (0~1)
    public void SetMasterVolume(float vol)
    {
        masterVolume = Mathf.Clamp01(vol);
        // 기존 BGM 볼륨 즉시 업데이트
        bgmPlayer.volume = bgmVolume * masterVolume;
    }
}
