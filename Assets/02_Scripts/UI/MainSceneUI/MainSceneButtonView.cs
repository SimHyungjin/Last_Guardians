using System;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneButtonView : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button bookBtn;
    [SerializeField] private Button soundOptionBtn;
    [SerializeField] private Button idleBtn;
    [SerializeField] private Button TowerUpgradeBtn;


    [SerializeField] private IdleRewardPopupUI idlePopup;

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
            mainSceneManager.ShowPanel("EquipTutorial",null,false);
        });

        bookBtn.onClick.AddListener(() =>
        {
            mainSceneManager.ShowPanel("Book");
        });

        soundOptionBtn.onClick.AddListener(() =>
        {
            mainSceneManager.ShowPanel("SoundOption");
        });

        TowerUpgradeBtn.onClick.AddListener(() =>
        {
            mainSceneManager.ShowPanel("TowerUpgrade");
            mainSceneManager.ShowPanel("UpgradeTutorial", null, false);
        });
    
        idleBtn.onClick.AddListener(() =>
        {                  
            mainSceneManager.ShowPanel("IdleRewardPopup");
        });
    }

    private void ToGameScene()
    {
        GameManager.Instance.PlayerManager.SaveEquipmentStat(MainSceneManager.Instance.equipment.InfoToPlayer());
        SaveSystem.SaveGame();
    }
}
