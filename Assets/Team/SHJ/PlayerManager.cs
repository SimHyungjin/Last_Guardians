public class PlayerManager
{
    public Player player;
    public PlayerController playerController;
    public PlayerInputHandler playerHandler;

    public PlayerData playerData;

    public void Init()
    {
        if (player == null) player = new();

        playerController = Utils.InstantiatePrefabFromResource("Player").GetComponent<PlayerController>();
        playerHandler = playerController.GetComponent<PlayerInputHandler>();

        player.Init();
        playerController.Init(player);
        playerHandler.Init();
    }
}
