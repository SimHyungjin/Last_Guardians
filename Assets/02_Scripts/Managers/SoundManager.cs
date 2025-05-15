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

    // 볼륨 값(0~1)
    private float sfxVolume;
    private float bgmVolume;
    private float masterVolume;

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

        // 1) 저장된 볼륨 설정 불러오기
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 0.3f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 0.3f);
        masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 0.3f);

        // 2) 사운드 딕셔너리 초기화
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
            soundDict[clip.name] = clip;

        // 3) BGM 플레이어 초기화
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume * masterVolume;

        // 4) SFX 오디오소스 풀 초기화
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.enabled = false;
            audioSourcePool.Enqueue(src);
        }

        // 5) 초기 볼륨 즉시 반영
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
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
        src.volume = sfxVolume * masterVolume;
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
        bgmPlayer.volume = bgmVolume * masterVolume;
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
        PlayerPrefs.SetFloat("SFX_VOLUME", sfxVolume);
        PlayerPrefs.Save();
    }

    // BGM 볼륨 설정 (0~1)
    public void SetBGMVolume(float vol)
    {
        bgmVolume = Mathf.Clamp01(vol);
        bgmPlayer.volume = bgmVolume * masterVolume;
        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.Save();
    }

    // 전체(마스터) 볼륨 설정 (0~1)
    public void SetMasterVolume(float vol)
    {
        masterVolume = Mathf.Clamp01(vol);
        bgmPlayer.volume = bgmVolume * masterVolume;
        PlayerPrefs.SetFloat("MASTER_VOLUME", masterVolume);
        PlayerPrefs.Save();
    }
}
