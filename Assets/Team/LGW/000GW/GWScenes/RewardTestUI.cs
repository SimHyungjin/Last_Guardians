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
        //wavePlusButton.onClick.AddListener(OnWavePlusClicked);
        getRewardButton.onClick.AddListener(OnGetRewardClicked);

        UpdateWaveText();
    }

    void OnWavePlusClicked()
    {
        MonsterManager.Instance.ForceAddWave(); // 이 함수는 아래 추가됨
        UpdateWaveText();
    }

    void OnGetRewardClicked()
    {
         
        RewardManager.Instance.GiveRewardForWave(10);
    }

    void UpdateWaveText()
    {
        //waveText.text = $"현재 웨이브: {MonsterManager.Instance.currentWaveIndex}";
    }
}
