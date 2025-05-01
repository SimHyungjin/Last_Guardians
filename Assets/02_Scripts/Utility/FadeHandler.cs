using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class FadeHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("Blink ����")]
    public bool enableBlink = false;      // ������ ��� ����
    [Tooltip("������ 1ȸ �ֱ�(��)")]
    public float blinkDuration = 1f;

    [Header("Fade ����")]
    public bool enableFade = false;       // ���̵� ��� ����
    [Tooltip("���̵� �ð�(��)")]
    public float fadeDuration = 1f;
    [Tooltip("���⿡ FadeImage ������Ʈ�� CanvasGroup�� �巡���ϼ���")]
    public CanvasGroup targetCanvasGroup;

    [Header("Ŭ�� ��� �ε�")]
    public bool directLoadOnClick = false;
    public string directSceneName;

    [Header("Ŭ�� �� ���̵� �� �ε�")]
    public bool fadeLoadOnClick = false;
    public string fadeSceneName;

    private Image _targetImage;
    private CanvasGroup _cg;

    void Awake()
    {
        // ���̵� ����� ���ٸ� CanvasGroup ����
        if (enableFade)
            SetupCanvasGroup();
    }

    void Start()
    {
        // ������ ����� ���ٸ� Blink Ʈ�� ����
        if (enableBlink)
            SetupBlink();
    }

    /// <summary>
    /// targetCanvasGroup �� Inspector���� �Ҵ�Ǿ� ������ �װ� ����,
    /// ������ �ڱ� �ڽ�(GetComponent)���� ã�Ƽ� ����.
    /// Alpha�� ���� �� ���� ����(0)����.
    /// </summary>
    void SetupCanvasGroup()
    {
        _cg = targetCanvasGroup != null
            ? targetCanvasGroup
            : GetComponent<CanvasGroup>();

        // ���� �׷��� null �̸� �߰� ����
        if (_cg == null)
            _cg = gameObject.AddComponent<CanvasGroup>();

        _cg.alpha = 0f;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
    }

    /// <summary>
    /// Blink�� Ʈ�� ����.
    /// ���̵嵵 ���� ���� CanvasGroup ����, �ƴϸ� Image ������Ʈ�� ó��.
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

    // IPointerClickHandler: Ŭ�� �ǳ� � ���̸� ��ü���� Ŭ�� �� ȣ��˴ϴ�.
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

    // UI Button OnClick() �� ���� �޼���
    public void OnClickDirectLoad()
    {
        SceneManager.LoadScene(directSceneName);
    }

    public void OnClickFadeLoad()
    {
        StartFadeAndLoad(fadeSceneName);
    }

    /// <summary>
    /// ���� ���̵� �ƿ� �� �� ��ȯ ó��.
    /// targetCanvasGroup.alpha 0��1
    /// </summary>
    public void StartFadeAndLoad(string scene)
    {
        if (enableFade && _cg != null)
        {
            // Blocks Raycasts ���༭ Ŭ�� ����
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
