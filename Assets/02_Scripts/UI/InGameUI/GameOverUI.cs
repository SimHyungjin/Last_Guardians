using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    //게임결과
    [SerializeField] TextMeshProUGUI resultWaveText;
    [SerializeField] TextMeshProUGUI resultMonsterCountText;
    [SerializeField] TextMeshProUGUI reslutPlayerLvText;
    //게임보상
    [SerializeField] TextMeshProUGUI rewardGoldText;
    [SerializeField] TextMeshProUGUI rewardStoneText;
    [SerializeField] Image rewardEquipImage;
    [SerializeField] TextMeshProUGUI rewardEquipText;

    //버튼
    [SerializeField] Button retryBtn;
    [SerializeField] Button exitBtn;

    private void OnEnable()
    {
        RewardManager.Instance.GiveRewardForWave(MonsterManager.Instance.nowWave.WaveIndex);
        resultWaveText.text = $"wave : {MonsterManager.Instance.nowWave.WaveIndex}";
        resultMonsterCountText.text = $"잡은 몬스터 수 : {MonsterManager.Instance.MonsterKillCount}";
        reslutPlayerLvText.text = $"플레이어 레벨 : {InGameManager.Instance.level}";
        rewardGoldText.text = $"획득한 골드 : {RewardManager.Instance.Gold}";
        rewardStoneText.text = $"획득한 강화석 : {RewardManager.Instance.Stone}";
        if (RewardManager.Instance.Equip == 0)
        {
            rewardEquipImage.gameObject.SetActive(false);
            rewardEquipText.gameObject.SetActive(false);
        }
        else
        {
            rewardEquipImage.sprite = GameManager.Instance.ItemManager.GetItemInstanceByIndex(RewardManager.Instance.Equip).Data.Icon;
            rewardEquipText.text = GameManager.Instance.ItemManager.GetItemInstanceByIndex(RewardManager.Instance.Equip).Data.itemName;
        }
        retryBtn.onClick.AddListener(Retry);
        exitBtn.onClick.AddListener(Exit);
    }

    private void Retry()
    {
        SceneLoader.LoadSceneWithFade("GameScene", true);
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene("GameScene");
    }

    private void Exit()
    {
        SceneLoader.LoadSceneWithFade("MainScene", true);
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene("MainScene");
    }
}
