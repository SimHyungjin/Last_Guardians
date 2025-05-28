using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 이 스크립트는 캐릭터의 드래그 기반 이동을 담당합니다.
/// </summary>
public class PlayerMoveController : MonoBehaviour
{
    private bool canMove = true;
    private bool isSwiping = false;
    private NavMeshAgent agent;
    private PlayerModelView playerView;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    private void Start()
    {
        playerView = GameManager.Instance.PlayerManager.playerHandler.playerView;
    }
    public void Init()
    {
        agent.speed = GameManager.Instance.PlayerManager.playerStatus.moveSpeed;
    }

    private void FixedUpdate()
    {
        transform.position = agent.nextPosition;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SwipeStart()
    {
        if (isSwiping) return;
        isSwiping = true;
    }

    public void SwipeStop()
    {
        if (!isSwiping || !canMove) return;
        isSwiping = false;

        Vector3 target = InputManager.Instance.GetTouchWorldPosition();
        target.z = transform.position.z;

        agent.SetDestination(target);
        playerView.OnMove();

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(WaitUntilArrival());
    }

    private IEnumerator WaitUntilArrival()
    {
        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance) break;
                playerView.UpdateMoveDirection(agent.velocity);
            }
            yield return null;
        }

        playerView.OnIdle();
        moveCoroutine = null;
    }

    #region Joystick 이동
    public void SetJoystickInput(Vector2 inputDir)
    {
        if (!canMove || inputDir.sqrMagnitude < 0.01f)
        {
            agent.ResetPath();
            playerView.OnIdle();
            return;
        }

        Vector3 worldDir = new Vector3(inputDir.x, inputDir.y, 0).normalized;
        Vector3 moveTarget = transform.position + worldDir * 0.5f;

        agent.SetDestination(moveTarget);
        playerView.OnMove();
        playerView.UpdateMoveDirection(worldDir);
    }
    #endregion

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!value) agent.ResetPath();
    }
}
