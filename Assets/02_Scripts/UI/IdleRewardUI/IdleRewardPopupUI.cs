using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardPopupUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;
    [SerializeField] private TextMeshProUGUI nextRewardText;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject screenBlocker;

    private void Awake()
    {
        panel.SetActive(false);
        screenBlocker.SetActive(false);
    }

    private void Start()
    {
        claimButton.onClick.AddListener(OnClickClaim);
        closeButton.onClick.AddListener(ClosePopup);
    }

    private void Update()
    {
        if (panel.activeSelf)
            UpdateTexts();
    }

    public void OpenPopup()
    {
        panel.SetActive(true);
        screenBlocker.SetActive(true);
        transform.SetAsLastSibling();
        screenBlocker.transform.SetSiblingIndex(0);
        UpdateTexts();
    }

    public void ClosePopup()
    {
        panel.SetActive(false);
        screenBlocker.SetActive(false);
    }

    private void UpdateTexts()
    {
        var mgr = IdleRewardManager.Instance;
        goldText.text = $"골드 +{mgr.Gold}";
        stoneText.text = $"강화석 +{mgr.Stone}";
        var elapsed = mgr.TotalElapsed;
        elapsedTimeText.text = $"누적 시간: {Mathf.FloorToInt((float)elapsed.TotalHours)}시간";

        var next = mgr.NextRewardIn;
        nextRewardText.text = $"다음 보상까지: {Mathf.CeilToInt((float)next.TotalMinutes)}분 남음";

        claimButton.interactable = (mgr.Gold > 0 || mgr.Stone > 0) && next.TotalSeconds <= 0;
    }

    private void OnClickClaim()
    {
        IdleRewardManager.Instance.ClaimReward();
        UpdateTexts();
    }
}
