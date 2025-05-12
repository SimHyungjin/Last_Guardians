using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SoundManager : Singleton<SoundManager>
{
    public static SoundManager instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips; // 오디오 클립 배열

    [Header("Object Pool Settings")]
    [SerializeField] private int poolSize = 30;           // 풀 크기

    private Dictionary<string, AudioClip> soundDict;      // SFX와 BGM을 저장할 Dictionary
    private Queue<AudioSource> audioSourcePool;           // 오브젝트 풀

    private AudioSource bgmPlayer;                        // BGM 재생용 AudioSource
    private float sfxVolume;
    private float bgmVolume;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시 클릭 효과음 재생
        if (Input.GetMouseButtonDown(0))
        {
            PlayClickSound();
        }
    }

    /// <summary>
    /// 클릭 효과음 재생
    /// </summary>
    public void PlayClickSound()
    {
        PlaySFX("Click");
    }

    /// <summary>
    /// 최초 세팅
    /// </summary>
    private void Init()
    {
        // Dictionary 초기화
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
        {
            soundDict[clip.name] = clip;
        }

        // BGM 플레이어 초기화
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;

        InitPool();
    }

    /// <summary>
    /// 오브젝트 풀 초기화
    /// </summary>
    private void InitPool()
    {
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.enabled = false;
            audioSourcePool.Enqueue(source);
        }
    }

    /// <summary>
    /// SFX 재생 (Object Pooling 사용)    /// </summary>
    /// <param name="soundName">사운드 이름</param>
    public void PlaySFX(string soundName)
    {
        if (soundDict.TryGetValue(soundName, out var clip))
        {
            if (audioSourcePool.Count > 0)
            {
                AudioSource source = audioSourcePool.Dequeue();
                source.clip = clip;
                source.volume = sfxVolume;
                source.enabled = true;
                source.Play();

                StartCoroutine(ReturnToPool(source, clip.length));
            }
            else
            {
                // 풀에 오디오 소스가 없을 경우, 새로 생성하여 사용
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.clip = clip;
                newSource.volume = sfxVolume;
                newSource.playOnAwake = false;
                newSource.enabled = true;
                newSource.Play();

                // 새로 생성한 소스는 재사용 후 풀에 다시 넣을 수 있도록 코루틴을 사용
                StartCoroutine(ReturnToPool(newSource, clip.length));
            }
        }
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    /// <param name="bgmName">사운드 이름</param>
    public void PlayBGM(string bgmName)
    {
        if (soundDict.TryGetValue(bgmName, out var clip))
        {
            if (bgmPlayer.clip != clip)
            {
                bgmPlayer.clip = clip;
                bgmPlayer.volume = bgmVolume;
                bgmPlayer.Play();
            }
        }
    }

    /// <summary>
    /// 현재 재생 중인 BGM 정지
    /// </summary>
    public void StopBGM()
    {
        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
        }
    }

    /// <summary>
    /// 재생 끝난 소스 풀로 되돌리는 과정
    /// </summary>
    /// <param name="source"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.enabled = false;
        audioSourcePool.Enqueue(source);
    }

    /// <summary>
    /// SFX 볼륨 조절
    /// </summary>
    /// <param name="volume">0~1 사이의 볼륨 값</param>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// BGM 볼륨 조절
    /// </summary>
    /// <param name="volume">0~1 사이의 볼륨 값</param>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmPlayer.volume = bgmVolume;
    }
}
