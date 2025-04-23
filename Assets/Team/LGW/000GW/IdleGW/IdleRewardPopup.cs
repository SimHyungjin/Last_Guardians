using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardPopup : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI equipText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        panel.SetActive(false);
        claimButton.onClick.AddListener(OnClickClaim);
        closeButton.onClick.AddListener(() => panel.SetActive(false));
    }

    private void Update()
    {
        if (panel.activeSelf)
            UpdateTexts();
    }

    public void OpenPopup()
    {
        panel.SetActive(true);
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        goldText.text = $"골드 +{IdleRewardManager.Instance.Gold}";
        stoneText.text = $"강화석 +{IdleRewardManager.Instance.Stone}";
        equipText.text = $"장비 +{IdleRewardManager.Instance.Equip}";

        var time = IdleRewardManager.Instance.GetElapsedTime();
        elapsedTimeText.text = $"누적 시간: {time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    private void OnClickClaim()
    {
        IdleRewardManager.Instance.ClaimReward();
        UpdateTexts();
    }
}
