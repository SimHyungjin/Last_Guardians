public class GameManager : Singleton<GameManager>
{
    public ItemManager ItemManager { get; private set; } = new();

    public int gold = 1000000;
    public int upgradeStones = 1000000;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ItemManager.LoadAllItemes();
    }

    public void AddStatData(Equipment equip)
    {

    }
}
