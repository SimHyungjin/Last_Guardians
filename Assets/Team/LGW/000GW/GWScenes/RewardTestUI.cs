using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardTestUI : MonoBehaviour
{
    public Button wavePlusButton;
    public Button getRewardButton;
    
    public TextMeshProUGUI waveText;

    private void Start()
    {
        wavePlusButton.onClick.AddListener(OnWavePlusClicked);
        getRewardButton.onClick.AddListener(OnGetRewardClicked);

        UpdateWaveText();
    }

    void OnWavePlusClicked()
    {
        MonsterManager.Instance.ForceAddWave(); // �� �Լ��� �Ʒ� �߰���
        UpdateWaveText();
    }

    void OnGetRewardClicked()
    {
         
        RewardManager.Instance.GiveRewardForWave(MonsterManager.Instance.currentWaveIndex);
    }

    void UpdateWaveText()
    {
        waveText.text = $"���� ���̺�: {MonsterManager.Instance.currentWaveIndex}";
    }
}
