using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ItemManager ItemManager { get; private set; } = new();

    public int gold = 1000000;
    public int upgradeStones = 1000000;

    public EquipmentStats stats = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ItemManager.LoadAllItems();
    }
}
