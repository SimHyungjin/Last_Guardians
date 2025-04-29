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
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isAttacking = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        weaponHandler.attackActionEnter += OnAttack;
        weaponHandler.attackActionExit += ExitAttack;
    }

    public void ChangeState(PlayerAnimState state, Vector2? attackDir = null)
    {
        if (curState == state) return;
        curState = state;

        switch (state)
        {
            case PlayerAnimState.Idle:
                EnterIdle();
                break;
            case PlayerAnimState.Move:
                EnterMove();
                break;
            case PlayerAnimState.Attack:
                EnterAttack(attackDir.Value);
                break;
            case PlayerAnimState.Stun:
                EnterStun();
                break;
        }
    }

    public void UpdateMoveDirection(Vector2 moveDir)
    {
        if (moveDir.x == 0 || isAttacking) return;
        bool flip = moveDir.x < 0;
        spriteRenderer.flipX = flip;
        weaponHandler?.SetFlip(flip);
    }

    public void OnIdle() => ChangeState(PlayerAnimState.Idle);
    public void OnMove() => ChangeState(PlayerAnimState.Move);
    public void OnAttack(Vector2 targetPos) => ChangeState(PlayerAnimState.Attack, targetPos);
    public void OnStun() => ChangeState(PlayerAnimState.Stun);

    public void OnStateEnd()
    {
        if (isMoving) OnMove();
        else OnIdle();
    }

    private void EnterIdle()
    {
        isMoving = false;
        animator.SetBool("IsMove", false);
    }

    private void EnterMove()
    {
        isMoving = true;
        animator.SetBool("IsMove", true);
    }

    private void EnterAttack(Vector2 targetPos)
    {
        UpdateMoveDirection(targetPos - (Vector2)transform.position);
        isAttacking = true;
    }

    private void ExitAttack()
    {
        isAttacking = false;
        OnStateEnd();
    }

    private void EnterStun()
    {

    }
}
