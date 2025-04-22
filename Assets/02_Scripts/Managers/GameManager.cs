using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ItemManager ItemManager { get; private set; } = new();

    public int gold = 1000000;
    public int upgradeStones = 1000000;

    public EquipmentStats stats = new();


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        ItemManager.LoadAllItems();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home_SHJ_Scene")
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
