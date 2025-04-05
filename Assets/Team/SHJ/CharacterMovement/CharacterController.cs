using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;
    public AttackController attackController;
    public MoveController moveController;

    // TODO: 임시용 키보드 입력
    public KeyboardMoveTest keyboardMoveTest;

    private void Awake()
    {
        attackController = GetComponent<AttackController>();
        moveController = GetComponent<MoveController>();

        // TODO: 임시용 키보드 입력
        keyboardMoveTest = GetComponent<KeyboardMoveTest>();

    }

    public void Init(Character _character)
    {
        character = _character;
        attackController.Init(character);
        moveController.Init(character);

        // TODO: 임시용 키보드 입력
        keyboardMoveTest.Init(character);
    }
}
