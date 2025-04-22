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

    //버튼
    [SerializeField] Button retryBtn;
    [SerializeField] Button exitBtn;

    private void OnEnable()
    {
        resultWaveText.text = $"wave : {MonsterManager.Instance.nowWave.WaveIndex}";
        resultMonsterCountText.text = $"잡은 몬스터 수 : {MonsterManager.Instance.MonsterKillCount}";
        reslutPlayerLvText.text = $"플레이어 레벨 : {InGameManager.Instance.level}";
        retryBtn.onClick.AddListener(Retry);
        exitBtn.onClick.AddListener(Exit);
    }

    private void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Exit()
    {
        SceneManager.LoadScene("MainScene");
    }
}
