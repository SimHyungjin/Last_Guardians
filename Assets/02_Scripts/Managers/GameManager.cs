public class GameManager : Singleton<GameManager>
{
    public int gold = 1000000;
    public int upgradeStones = 1000000;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
