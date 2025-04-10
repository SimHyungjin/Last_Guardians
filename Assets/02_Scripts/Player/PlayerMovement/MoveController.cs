using System.Collections;
using UnityEngine;

/// <summary>
/// 이 스크립트는 캐릭터의 드래그 기반 이동을 담당합니다.
/// </summary>
public class MoveController : MonoBehaviour
{
    private Vector2 endPos;
    private bool isSwiping = false;

    private Coroutine moveCoroutine;

    /// <summary>
    /// 드래그 입력이 시작될 때 호출됩니다.
    /// 화면 클릭시 위치값을 저장합니다.
    /// </summary>
    public void SwipeStart()
    {
        if (isSwiping) return;
        isSwiping = true;
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    /// <summary>
    /// 드래그 입력이 끝날 때 호출됩니다.
    /// </summary>
    public void SwipeStop()
    {
        if (!isSwiping) return;

        isSwiping = false;
        endPos = InputManager.Instance.GetTouchWorldPosition();
        moveCoroutine = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        if (endPos == null) yield break;
        while (Vector2.Distance(transform.position, endPos) > 0.01f)
        {
            Vector2 direction = endPos - (Vector2)transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            transform.position = Vector2.MoveTowards(transform.position, endPos, InGameManager.Instance.playerManager.player.playerData.moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = endPos;
    }
}
