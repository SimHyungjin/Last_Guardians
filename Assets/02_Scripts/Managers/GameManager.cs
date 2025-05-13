using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ItemManager ItemManager { get; private set; } = new();

    public int gold = 0;
    public int upgradeStones = 0;

    public EquipmentInfo stats = new();

    public int NowTime { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ItemManager.LoadAllItems();

        var idle = IdleRewardManager.Instance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            StartCoroutine(DelayLoadGame());
        }
    }

    private IEnumerator DelayLoadGame()
    {
        yield return null;
        SaveSystem.LoadGame();
        
    }
}
