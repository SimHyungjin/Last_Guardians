public class PlayerManager
{
    public Player player;
    public PlayerHandler playerHandler;
    public PlayerInputHandler playerInputHandler;

    public void Init()
    {
        if (player == null) player = new();

        playerHandler = Utils.InstantiatePrefabFromResource("Player/Player").GetComponent<PlayerHandler>();
        playerInputHandler = playerHandler.GetComponent<PlayerInputHandler>();

        player.Init();
        player.SetStatus();
        playerHandler.Init(player);
        playerInputHandler.Init();
    }
}
