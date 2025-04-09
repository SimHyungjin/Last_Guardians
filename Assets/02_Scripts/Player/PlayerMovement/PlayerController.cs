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
    }

    public void Init(Player _player)
    {
        player = _player;
        attackController.Init(player);
    }
}
