using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardPopupUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;
    [SerializeField] private TextMeshProUGUI nextRewardText;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        // 보상 받기
        claimButton.onClick.AddListener(() =>
        {
            IdleRewardManager.Instance.ClaimReward();
            UpdateTexts();
        });

        // 닫기
        closeButton.onClick.AddListener(() =>
        {
            MainSceneManager.Instance.HidePanel("IdleRewardPopup");
        });
    }

    private void Update()
    {
        // 패널이 켜져 있을 때만 매 프레임 텍스트 갱신
        if (!gameObject.activeSelf) return;
        UpdateTexts();
    }

    /// <summary>
    /// IdleRewardManager에서 계산된 값을 가져와
    /// 각 텍스트에 반영합니다.
    /// </summary>
    private void UpdateTexts()
    {
        var mgr = IdleRewardManager.Instance;
        goldText.text = $"골드 +{mgr.Gold}";
        stoneText.text = $"강화석 +{mgr.Stone}";

        var elapsed = mgr.TotalElapsed;
        int hours = Mathf.FloorToInt((float)elapsed.TotalHours);
        elapsedTimeText.text = $"누적 시간: {hours}시간";

        var next = mgr.NextRewardIn;
        nextRewardText.text = $"다음 보상까지: {next.Hours:D2}:{next.Minutes:D2}:{next.Seconds:D2}";

        claimButton.interactable = (mgr.Gold > 0 || mgr.Stone > 0);
    }
}
