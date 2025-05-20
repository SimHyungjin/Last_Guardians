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

        inventoryBtn.onClick.AddListener(() => SoundManager.Instance.PlaySFX("PopUp"));
        bookBtn.onClick.AddListener(() => SoundManager.Instance.PlaySFX("PopUp"));
        soundOptionBtn.onClick.AddListener(() => SoundManager.Instance.PlaySFX("PopUp"));
        idleBtn.onClick.AddListener(() => SoundManager.Instance.PlaySFX("PopUp"));
        TowerUpgradeBtn.onClick.AddListener(() => SoundManager.Instance.PlaySFX("PopUp"));
        startBtn.onClick.AddListener(() => SoundManager.Instance.PlaySFX("FadeOut"));



        startBtn.onClick.AddListener(() =>
        {
            SceneLoader.LoadSceneWithFade("GameScene", true, ToGameScene);
        });

        inventoryBtn.onClick.AddListener(() =>
        {
            mainSceneManager.LoadInventory(mainSceneManager.canvas);

            if (PlayerPrefs.GetInt("EquipTutorial") != 1)
                mainSceneManager.ShowPanel("EquipTutorial", null, false);
            else mainSceneManager.ShowPanel("InventoryGroup");

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

            if (PlayerPrefs.GetInt("UpgradeTutorial") != 1)
                mainSceneManager.ShowPanel("UpgradeTutorial", null, false);
            else mainSceneManager.ShowPanel("TowerUpgrade");
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
