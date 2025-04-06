public class PlayerManager
{
    public Player player;
    public PlayerController playerController;
    public PlayerInputHandler playerHandler;

    public PlayerData playerData;

    public void Init()
    {
        if (playerData == null)
            playerData = new(10,10,1,10,10,1,10,10,10,10,10);
        if (player == null)
            player = new();

        playerController = Utils.InstantiatePrefabFromResource("Player").GetComponent<PlayerController>();
        playerHandler = playerController.GetComponent<PlayerInputHandler>();

        player.Init(playerData);
        playerController.Init(player);
        playerHandler.Init();
    }
}
