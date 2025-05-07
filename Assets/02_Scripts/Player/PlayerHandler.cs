using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public Player player {  get; private set; }
    public PlayerAttackController attackController { get; private set; }
    public PlayerMoveController moveController { get; private set; }
    public PlayerBuffHandler playerBuffHandler { get; private set; }
    public PlayerView playerView { get; private set; }
    public PlayerWeaponHandler weaponHandler { get; private set; }
    public PlayerObstacleDebuff playerObstacleDebuff { get; private set; }

    private void Awake()
    {
        attackController = GetComponent<PlayerAttackController>();
        moveController = GetComponent<PlayerMoveController>();
        playerBuffHandler = GetComponent<PlayerBuffHandler>();

        playerView = GetComponentInChildren<PlayerView>();
        weaponHandler = GetComponentInChildren<PlayerWeaponHandler>();
        playerObstacleDebuff = GetComponentInChildren<PlayerObstacleDebuff>();

        gameObject.transform.position = new Vector3(0.5f, -2f, 0);
    }

    public void Init(Player _player)
    {
        player = _player;
        attackController.Init();
        moveController.Init();
        playerBuffHandler.Init();
        playerObstacleDebuff.Init();
    }
}
