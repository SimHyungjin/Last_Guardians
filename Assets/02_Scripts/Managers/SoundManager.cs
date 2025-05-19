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
    [SerializeField] private int maxPoolSize = 100;   // 풀에서 허용할 최대 AudioSource 수

    // 이름 → 클립 매핑
    private Dictionary<string, AudioClip> soundDict;

    // SFX용 AudioSource 풀
    private Queue<AudioSource> audioSourcePool;
    private int currentPoolCapacity;  // 현재 생성된 AudioSource 개수

    // BGM 전용 AudioSource
    private AudioSource bgmPlayer;

    // SFX 루프 전용 AudioSource
    private AudioSource loopSource;

    // 볼륨 값 (0~1)
    private float sfxVolume;
    private float bgmVolume;
    private float masterVolume;

    // 클릭 사운드 바인딩용 InputSystem
    private PointerInput pointerInput;
    private PointerInput.PointerActions pointerAction;

    private void Awake()
    {
        Init();

        // 클릭 사운드 바인딩
        pointerInput = new PointerInput();
        pointerAction = pointerInput.Pointer;
        pointerInput.Enable();
        pointerAction.Pressed.started += _ => PlaySFX("ClickSound");
    }

    private void Start()
    {
        // 메뉴/시작 씬용 기본 BGM
        PlayBGM("PianoInstrumental1", loop: true);
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        // 저장된 볼륨 불러오기
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 0.3f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 0.3f);
        masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 0.3f);

        // AudioClip 딕셔너리 생성
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
            soundDict[clip.name] = clip;

        // BGM 전용 AudioSource
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;

        // SFX용 풀 생성
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.enabled = false;
            audioSourcePool.Enqueue(src);
        }
        currentPoolCapacity = poolSize;

        // 초기 볼륨 적용
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    // ────────────────────────────────────────────
    // SFX: 단발 재생
    // ────────────────────────────────────────────
    public void PlaySFX(string soundName)
    {
        if (!soundDict.TryGetValue(soundName, out var clip))
        {
            Debug.LogError($"[SoundManager] SFX clip not found: {soundName}");
            return;
        }

        AudioSource src = null;

        if (audioSourcePool.Count > 0)
        {
            // 풀에 남아있는 AudioSource 사용
            src = audioSourcePool.Dequeue();
        }
        else if (currentPoolCapacity < maxPoolSize)
        {
            // 최대치 미만일 때 새로 생성
            src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            currentPoolCapacity++;
        }
        else
        {
            // 최대치 도달 시 재생 스킵
            Debug.LogWarning($"[SoundManager] 최대 SFX 소스 수({maxPoolSize})에 도달했습니다. 스킵: {soundName}");
            return;
        }

        src.clip = clip;
        src.volume = sfxVolume * masterVolume;
        src.loop = false;
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

    // ────────────────────────────────────────────
    // SFX: 루프 재생 시작
    // ────────────────────────────────────────────
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

    // ────────────────────────────────────────────
    // SFX: 루프 재생 중지
    // ────────────────────────────────────────────
    public void StopSFXLoop()
    {
        if (loopSource != null && loopSource.isPlaying)
            loopSource.Stop();
    }

    // ────────────────────────────────────────────
    // 기존 StopSFX 호출 호환용
    // ────────────────────────────────────────────
    public void StopSFX(string soundName)
    {
        if (loopSource != null
            && loopSource.isPlaying
            && loopSource.clip != null
            && loopSource.clip.name == soundName)
        {
            loopSource.Stop();
        }
    }

    // 파라미터 없는 호출용 오버로드
    public void StopSFX()
    {
        if (loopSource != null && loopSource.isPlaying)
            loopSource.Stop();
    }

    // ────────────────────────────────────────────
    // BGM: 재생 (loop=true 기본)
    // ────────────────────────────────────────────
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
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();
    }

    // ────────────────────────────────────────────
    // BGM 재생 상태/길이 조회
    // ────────────────────────────────────────────
    public bool IsBgmPlaying() => bgmPlayer.isPlaying;

    public float GetBgmLength(string bgmName)
    {
        return soundDict.TryGetValue(bgmName, out var clip)
            ? clip.length
            : 0f;
    }

    // ────────────────────────────────────────────
    // 볼륨 설정
    // ────────────────────────────────────────────
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
