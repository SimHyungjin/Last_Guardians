using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public PlayerStatus player {  get; private set; }

    public PlayerInputHandler playerInputHandler { get; private set; }
    public PlayerMoveController moveController { get; private set; }
    public PlayerAttackController attackController { get; private set; }

    public PlayerBuffHandler playerBuffHandler { get; private set; }
    public PlayerObstacleDebuff playerObstacleDebuff { get; private set; }

    public PlayerModelView playerView { get; private set; }
    public PlayerWeaponController weaponHandler { get; private set; }



    private void Awake()
    {
        playerInputHandler = GetComponentInChildren<PlayerInputHandler>();
        moveController = GetComponent<PlayerMoveController>();
        attackController = GetComponent<PlayerAttackController>();
        
        playerBuffHandler = GetComponent<PlayerBuffHandler>();
        playerObstacleDebuff = GetComponentInChildren<PlayerObstacleDebuff>();

        playerView = GetComponentInChildren<PlayerModelView>();
        weaponHandler = GetComponentInChildren<PlayerWeaponController>();
    }

    public void Init(PlayerStatus _player)
    {
        player = _player;

        playerInputHandler.Init();
        moveController.Init();
        attackController.Init();
        
        playerBuffHandler.Init();
        playerObstacleDebuff.Init();

        gameObject.transform.position = new Vector3(0.5f, -2f, 0);
    }
}
