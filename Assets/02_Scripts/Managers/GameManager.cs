using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ItemManager ItemManager { get; private set; } = new();
    public PlayerManager PlayerManager { get; private set; } = new();

    public int gold = 0;
    public int upgradeStones = 0;

    //public int InGameTutorial = PlayerPrefs.GetInt("InGameTutorial");
    //public int EquipTutorial = PlayerPrefs.GetInt("EquipTutorial");
    //public int UpgradeTutorial = PlayerPrefs.GetInt("UpgradeTutorial");

    public int NowTime { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (GetComponent<ServiceInitializer>() == null)
            gameObject.AddComponent<ServiceInitializer>();
        ItemManager.LoadAllItems();

        var idle = IdleRewardManager.Instance;
        PlayerManager.InitBaseStat();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (GetComponent<TowerUpgrade>() == null)
        {
            gameObject.AddComponent<TowerUpgrade>();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
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
