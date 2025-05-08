using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardPopup : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;
    [SerializeField] private TextMeshProUGUI nextRewardText;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject ScreenBlocker;

    private void Awake()
    {
       
        panel.SetActive(false);
        ScreenBlocker.SetActive(false);
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
        ScreenBlocker.SetActive(true);

        
        transform.SetAsLastSibling();

      
        ScreenBlocker.transform.SetSiblingIndex(0);

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

        var elapsed = IdleRewardManager.Instance.TotalElapsed;
        elapsedTimeText.text = $"���� �ð�: {Mathf.FloorToInt((float)elapsed.TotalHours)}�ð�";

        var next = IdleRewardManager.Instance.NextRewardIn;
        nextRewardText.text = $"���� �������: {Mathf.FloorToInt((float)next.TotalMinutes)}�� ����";
    }

    private void OnClickClaim()
    {
        IdleRewardManager.Instance.ClaimReward();
        UpdateTexts();
    }
}
