using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인 씬의 버튼을 관리하는 클래스입니다.
/// </summary>
public class MainSceneButtonView : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button bookBtn;
    [SerializeField] private Button soundOptionBtn;
    [SerializeField] private Button idleBtn;

    public void Init(MainSceneManager mainSceneManager)
    {
        startBtn.onClick.AddListener(() =>
        {
            SceneLoader.LoadSceneWithFade("GameScene", true, ToGameScene);
        });

        inventoryBtn.onClick.AddListener(() =>
        {
            mainSceneManager.LoadInventory(mainSceneManager.canvas);
            mainSceneManager.ShowPanel("InventoryGroup");
        });

        bookBtn.onClick.AddListener(() =>
        {
            mainSceneManager.ShowPanel("Book");
        });

        soundOptionBtn.onClick.AddListener(() =>
        {
            mainSceneManager.ShowPanel("SoundOption");
        });

        idleBtn.onClick.AddListener(() =>
        {
            mainSceneManager.ShowPanel("IdleRewardPopup", null, false); 
        });

    }

    /// <summary>
    /// 게임 씬으로 전환합니다. 게임 씬을 로드하고, 장비 정보를 플레이어에게 전달합니다.
    /// </summary>
    private void ToGameScene()
    {
        GameManager.Instance.stats = MainSceneManager.Instance.equipment.InfoToPlayer();
        SaveSystem.SaveGame();
    }
}
