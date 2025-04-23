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
    [SerializeField] private GameObject ScreenBlocker;

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
        ScreenBlocker.SetActive(true);
        UpdateTexts();
    }
    public void ClosePopup()
    {
        panel.SetActive(false);
        ScreenBlocker.SetActive(false); 
    }

    private void UpdateTexts()
    {
        goldText.text = $"��� +{IdleRewardManager.Instance.Gold}";
        stoneText.text = $"��ȭ�� +{IdleRewardManager.Instance.Stone}";
        equipText.text = $"��� +{IdleRewardManager.Instance.Equip}";

        var time = IdleRewardManager.Instance.GetElapsedTime();
        elapsedTimeText.text = $"���� �ð�: {time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    private void OnClickClaim()
    {
        IdleRewardManager.Instance.ClaimReward();
        UpdateTexts();
    }
}
