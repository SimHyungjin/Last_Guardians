using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player {  get; private set; }
    public AttackController attackController { get; private set; }
    public MoveController moveController { get; private set; }

    // TODO: 임시용 키보드 입력
    private KeyboardMoveTest keyboardMoveTest;

    private void Awake()
    {
        attackController = GetComponent<AttackController>();
        moveController = GetComponent<MoveController>();

        // TODO: 임시용 키보드 입력
        keyboardMoveTest = GetComponent<KeyboardMoveTest>();

    }

    public void Init(Player _player)
    {
        player = _player;
        attackController.Init(player);
        moveController.Init(player);

        // TODO: 임시용 키보드 입력
        keyboardMoveTest.Init(player);
    }
}
