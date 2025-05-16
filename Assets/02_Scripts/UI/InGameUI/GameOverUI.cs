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
    [SerializeField] private TextMeshProUGUI resultWaveText;
    [SerializeField] private TextMeshProUGUI resultMonsterCountText;
    [SerializeField] private TextMeshProUGUI resultPlayerLvText;

    // 게임 보상
    [SerializeField] private TextMeshProUGUI rewardGoldText;
    [SerializeField] private TextMeshProUGUI rewardStoneText;
    [SerializeField] private Image rewardEquipImage;
    [SerializeField] private TextMeshProUGUI rewardEquipText;

    // 버튼
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button exitBtn;

    // 등급별 텍스트 색상 매핑
    private static readonly Dictionary<ItemGrade, string> GradeColorHex = new()
    {
        { ItemGrade.Normal, "#000000" }, // 검정
        { ItemGrade.Rare,   "#0000FF" }, // 파랑
        { ItemGrade.Unique, "#800080" }, // 보라
        { ItemGrade.Hero,   "#00FF00" }, // 초록
        { ItemGrade.Legend, "#FFFF00" }, // 노랑
    };

    private void OnEnable()
    {
        // 1) 보상 계산
        int wave = MonsterManager.Instance.nowWave.WaveIndex;
        RewardManager.Instance.GiveRewardForWave(wave);

        // 2) 결과 텍스트 세팅
        resultWaveText.text = $"wave : {wave}";
        resultMonsterCountText.text = $"잡은 몬스터 수 : {MonsterManager.Instance.MonsterKillCount}";
        resultPlayerLvText.text = $"플레이어 레벨 : {InGameManager.Instance.level}";

        // 3) 골드/강화석 텍스트
        rewardGoldText.text = $"획득한 골드 : {RewardManager.Instance.Gold}";
        rewardStoneText.text = $"획득한 강화석 : {RewardManager.Instance.Stone}";

        // 4) 장비 처리 (여러 개 나열 + 등급별 색상)
        var equips = RewardManager.Instance.EquipIndices;
        if (equips == null || equips.Count == 0)
        {
            rewardEquipImage.gameObject.SetActive(false);
            rewardEquipText.gameObject.SetActive(false);
        }
        else
        {
           
            var firstData = GameManager.Instance
                .ItemManager
                .GetItemInstanceByIndex(equips[0])
                .Data;
            rewardEquipImage.gameObject.SetActive(true);
            rewardEquipImage.sprite = firstData.Icon;

            
            var coloredNames = equips.Select(idx =>
            {
                var data = GameManager.Instance
                    .ItemManager
                    .GetItemInstanceByIndex(idx)
                    .Data;
                
                string hex = GradeColorHex.TryGetValue(data.ItemGrade, out var c) ? c : "#000000";
                return $"<color={hex}>{data.ItemName}</color>";
            });

            rewardEquipText.richText = true;
            rewardEquipText.gameObject.SetActive(true);
            rewardEquipText.text = string.Join(", ", coloredNames);
        }

        // 5) 버튼 리스너
        retryBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
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
