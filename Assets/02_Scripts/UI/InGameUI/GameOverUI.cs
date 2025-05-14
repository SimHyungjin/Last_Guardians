using System.Collections;
using System.Collections.Generic;
using System.Linq;              
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
        // 1) 보상 계산
        int wave = MonsterManager.Instance.nowWave.WaveIndex;
        RewardManager.Instance.GiveRewardForWave(wave);

        // 2) 결과 텍스트
        resultWaveText.text = $"wave : {wave}";
        resultMonsterCountText.text = $"잡은 몬스터 수 : {MonsterManager.Instance.MonsterKillCount}";
        reslutPlayerLvText.text = $"플레이어 레벨 : {InGameManager.Instance.level}";

        // 3) 골드/강화석
        rewardGoldText.text = $"획득한 골드 : {RewardManager.Instance.Gold}";
        rewardStoneText.text = $"획득한 강화석 : {RewardManager.Instance.Stone}";

        // 4) 장비 처리 (여러 개 나열)
        var equips = RewardManager.Instance.EquipIndices;
        if (equips == null || equips.Count == 0)
        {
            // 장비가 하나도 없으면 UI 숨기기
            rewardEquipImage.gameObject.SetActive(false);
            rewardEquipText.gameObject.SetActive(false);
        }
        else
        {
            // 첫 번째 장비 아이콘은 그대로 사용
            int firstIndex = equips[0];
            var firstItem = GameManager.Instance
                .ItemManager
                .GetItemInstanceByIndex(firstIndex)
                .Data;

            rewardEquipImage.gameObject.SetActive(true);
            rewardEquipImage.sprite = firstItem.Icon;

            // EquipIndices 전부 돌면서 이름만 추출 → "이름1, 이름2, 이름3"
            var names = equips
                .Select(idx =>
                    GameManager.Instance
                        .ItemManager
                        .GetItemInstanceByIndex(idx)
                        .Data.ItemName
                );

            rewardEquipText.gameObject.SetActive(true);
            rewardEquipText.text = string.Join(", ", names);
        }

        // 5) 버튼 리스너
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
