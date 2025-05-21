using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("Blink 설정")]
    public bool enableBlink = false;      // 깜빡임 사용 여부
    [Tooltip("깜빡임 1회 주기(초)")]
    public float blinkDuration = 1f;

    [Header("Fade 설정")]
    public bool enableFade = false;       // 페이드 사용 여부
    [Tooltip("페이드 시간(초)")]
    public float fadeDuration = 1f;
    [Tooltip("여기에 FadeImage 오브젝트의 CanvasGroup을 드래그하세요")]
    public CanvasGroup targetCanvasGroup;

    [Header("클릭 즉시 로드")]
    public bool directLoadOnClick = false;
    public string directSceneName;

    [Header("클릭 → 페이드 후 로드")]
    public bool fadeLoadOnClick = false;
    public string fadeSceneName;

    private Image _targetImage;
    private CanvasGroup _cg;

    void Awake()
    {
        // 페이드 기능을 쓴다면 CanvasGroup 세팅
        if (enableFade)
            SetupCanvasGroup();
    }

    void Start()
    {
        // 깜빡임 기능을 쓴다면 Blink 트윈 설정
        if (enableBlink)
            SetupBlink();
    }

    /// <summary>
    /// targetCanvasGroup 이 Inspector에서 할당되어 있으면 그걸 쓰고,
    /// 없으면 자기 자신(GetComponent)에서 찾아서 세팅.
    /// Alpha는 시작 시 완전 투명(0)으로.
    /// </summary>
    void SetupCanvasGroup()
    {
        _cg = targetCanvasGroup != null
            ? targetCanvasGroup
            : GetComponent<CanvasGroup>();

        // 만약 그래도 null 이면 추가 생성
        if (_cg == null)
            _cg = gameObject.AddComponent<CanvasGroup>();

        _cg.alpha = 0f;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    /// <summary>
    /// Blink용 트윈 세팅.
    /// 페이드도 같이 쓰면 CanvasGroup 으로, 아니면 Image 컴포넌트로 처리.
    /// </summary>
    void SetupBlink()
    {
        if (enableFade && _cg != null)
        {
            _cg.DOFade(0f, blinkDuration)
               .SetLoops(-1, LoopType.Yoyo)
               .SetEase(Ease.InOutSine);
        }
        else
        {
            _targetImage = GetComponent<Image>();
            if (_targetImage != null)
            {
                _targetImage.DOFade(0f, blinkDuration)
                            .SetLoops(-1, LoopType.Yoyo)
                            .SetEase(Ease.InOutSine);
            }
        }
    }

    // IPointerClickHandler: 클릭 판넬 등에 붙이면 전체영역 클릭 시 호출됩니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (directLoadOnClick)
        {
            SceneManager.LoadScene(directSceneName);
        }
        else if (fadeLoadOnClick)
        {
            StartFadeAndLoad(fadeSceneName);
        }
    }

    // UI Button OnClick() 용 직접 메서드
    public void OnClickDirectLoad()
    {
        SceneManager.LoadScene(directSceneName);
    }

    public void OnClickFadeLoad()
    {
        StartFadeAndLoad(fadeSceneName);
    }

    /// <summary>
    /// 실제 페이드 아웃 후 씬 전환 처리.
    /// targetCanvasGroup.alpha 0→1
    /// </summary>
    public void StartFadeAndLoad(string scene)
    {
        if (enableFade && _cg != null)
        {
            // Blocks Raycasts 켜줘서 클릭 막기
            _cg.blocksRaycasts = true;
            _cg.interactable = true;

            _cg.DOFade(1f, fadeDuration)
               .OnComplete(() => SceneManager.LoadScene(scene));
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }
}
