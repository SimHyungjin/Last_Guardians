using UnityEngine;

public enum PlayerAnimState
{
    Idle,
    Move,
    Attack,
    Stun,
}
public class PlayerView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private PlayerWeaponHandler weaponHandler;
    [SerializeField] private PlayerAnimState curState = PlayerAnimState.Idle;
    [SerializeField] private bool isMoving= false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void ChangeState(PlayerAnimState state)
    {
        if (curState == state) return;
        curState = state;

        switch (state)
        {
            case PlayerAnimState.Idle:
                IsIdle();
                break;
            case PlayerAnimState.Move:
                IsMove();
                break;
            case PlayerAnimState.Attack:
                IsAttack();
                break;
            case PlayerAnimState.Stun:
                IsStun();
                break;

        }
    }
    public void UpdateMoveDirection(Vector2 moveDir)
    {
        if (moveDir.x == 0) return;

        bool flip = moveDir.x < 0;
        spriteRenderer.flipX = flip; 

        if (weaponHandler != null)
            weaponHandler.SetFlip(flip);
    }

    public void OnIdle() => ChangeState(PlayerAnimState.Idle);
    public void OnMove() => ChangeState(PlayerAnimState.Move);
    public void OnAttack() => ChangeState(PlayerAnimState.Attack);
    public void OnStun() => ChangeState(PlayerAnimState.Stun);

    public void OnStateEnd()
    {
        if (isMoving) OnMove();
        else OnIdle();
    }

    private void IsIdle()
    {
        isMoving = false;
    }
    private void IsMove()
    {
        isMoving = true;
    }
    private void IsAttack()
    {
        
    }
    private void IsStun()
    {

    }
}
