using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player {  get; private set; }
    public PlayerAttackController attackController { get; private set; }
    public PlayerMoveController moveController { get; private set; }
    public PlayerBuffHandler playerBuffHandler { get; private set; }

    private void Awake()
    {
        attackController = GetComponent<PlayerAttackController>();
        moveController = GetComponent<PlayerMoveController>();
        playerBuffHandler = GetComponent<PlayerBuffHandler>();

        gameObject.transform.position = new Vector3(0.5f, -2f, 0);
    }

    public void Init(Player _player)
    {
        player = _player;
        attackController.Init(player);
        moveController.Init();
        playerBuffHandler.Init();
    }
}
