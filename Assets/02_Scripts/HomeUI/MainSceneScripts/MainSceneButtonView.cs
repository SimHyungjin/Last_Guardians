using UnityEngine;
using UnityEngine.UI;

public class MainSceneButtonView : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button bookBtn;
    [SerializeField] private Button soundOptionBtn;

    public void Init(MainSceneManager mainSceneManager)
    {
        startBtn.onClick.AddListener(() =>
        {
            SceneLoader.LoadSceneWithFade("GameScene", true, ToGameScene);
        });

        inventoryBtn.onClick.AddListener(() =>
        {
            mainSceneManager.LoadInventory(gameObject);
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
    }

    private void ToGameScene()
    {
        GameManager.Instance.stats = MainSceneManager.Instance.equipment.ToStats();
        SaveSystem.SaveGame();
    }
}
