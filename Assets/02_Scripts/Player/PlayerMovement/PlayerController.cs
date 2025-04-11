using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player {  get; private set; }
    public AttackController attackController { get; private set; }
    public MoveController moveController { get; private set; }

    private void Awake()
    {
        attackController = GetComponent<AttackController>();
        moveController = GetComponent<MoveController>();

        gameObject.transform.position = new Vector3(0.5f, -2f, 0);
    }

    public void Init(Player _player)
    {
        player = _player;
        attackController.Init(player);
    }
}
