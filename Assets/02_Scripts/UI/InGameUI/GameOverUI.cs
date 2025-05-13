using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    // 게임 결과
    [SerializeField] TextMeshProUGUI resultWaveText;
    [SerializeField] TextMeshProUGUI resultMonsterCountText;
    [SerializeField] TextMeshProUGUI reslutPlayerLvText;

    // 게임 보상
    [SerializeField] TextMeshProUGUI rewardGoldText;
    [SerializeField] TextMeshProUGUI rewardStoneText;
    [SerializeField] Image rewardEquipImage;
    [SerializeField] TextMeshProUGUI rewardEquipText;

    // 버튼
    [SerializeField] Button retryBtn;
    [SerializeField] Button exitBtn;

    private void OnEnable()
    {
        // 보상 계산
        int wave = MonsterManager.Instance.nowWave.WaveIndex;
        RewardManager.Instance.GiveRewardForWave(wave);

        // 결과 텍스트
        resultWaveText.text = $"wave : {wave}";
        resultMonsterCountText.text = $"잡은 몬스터 수 : {MonsterManager.Instance.MonsterKillCount}";
        reslutPlayerLvText.text = $"플레이어 레벨 : {InGameManager.Instance.level}";

        // 골드/강화석
        rewardGoldText.text = $"획득한 골드 : {RewardManager.Instance.Gold}";
        rewardStoneText.text = $"획득한 강화석 : {RewardManager.Instance.Stone}";

        // 장비 처리
        var equips = RewardManager.Instance.EquipIndices;
        if (equips == null || equips.Count == 0)
        {
            // 장비 없음
            rewardEquipImage.gameObject.SetActive(false);
            rewardEquipText.gameObject.SetActive(false);
        }
        else
        {
            // 최소 첫 번째 장비만 표시
            int firstIndex = equips[0];
            var itemData = GameManager.Instance
                .ItemManager
                .GetItemInstanceByIndex(firstIndex)
                .Data;

            rewardEquipImage.gameObject.SetActive(true);
            rewardEquipText.gameObject.SetActive(true);

            rewardEquipImage.sprite = itemData.Icon;
            rewardEquipText.text = itemData.ItemName;

           
        }

        // 버튼
        retryBtn.onClick.AddListener(Retry);
        exitBtn.onClick.AddListener(Exit);
    }

    private void Retry()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadSceneWithFade("GameScene", true);
    }

    private void Exit()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadSceneWithFade("MainScene", true);
    }
}
