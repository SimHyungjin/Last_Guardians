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
    private PlayerView playerView;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    public void Init()
    {
        playerView = InGameManager.Instance.playerManager.playerController.playerView;
        agent.speed = InGameManager.Instance.playerManager.player.playerData.moveSpeed;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
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

    public void SetCanMove(bool value)
    {
        canMove = value;
        if(!value) agent.ResetPath();
    }
}
