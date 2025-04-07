using System.Collections;
using UnityEngine;

/// <summary>
/// 이 스크립트는 캐릭터의 드래그 기반 이동을 담당합니다.
/// </summary>
public class MoveController : MonoBehaviour
{
    private Player player;

    private float responsiveness = 0.5f;
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isSwiping = false;

    private Coroutine dragCoroutine;

    /// <summary>
    /// 캐릭터 데이터를 주입합니다.
    /// </summary>
    public void Init(Player _player)
    {
        player = _player;
    }

    /// <summary>
    /// 드래그 입력이 시작될 때 호출됩니다.
    /// 화면 클릭시 위치값을 저장합니다.
    /// </summary>
    public void SwipeStart()
    {
        if (isSwiping) return;

        startPos = InputManager.Instance.GetTouchWorldPosition();
        isSwiping = true;

        //dragCoroutine = StartCoroutine(DragLoop());
    }

    /// <summary>
    /// 드래그 입력이 끝날 때 호출됩니다.
    /// 정보를 초기화합니다.
    /// </summary>
    public void SwipeStop()
    {
        if (!isSwiping) return;

        isSwiping = false;
        endPos = InputManager.Instance.GetTouchWorldPosition();
        //Move();

        //if (dragCoroutine != null)
        //{
        //    StopCoroutine(dragCoroutine);
        //    dragCoroutine = null;
        //}
    }

    //IEnumerator Move()
    //{
    //    if (startPos == null || endPos == null) yield break;
    //    while(Vector2.Distance(startPos, endPos) < 0.01f)
    //    transform.position = Vector2.MoveTowards(startPos, endPos, player.playerData.moveSpeed * Time.deltaTime);
    //    return null;
    //}

    /// <summary>
    /// 드래그 방향을 받아 캐릭터를 이동시키는 루프입니다.
    /// 일정 거리 이상일 경우 이동하며, 회전도 함께 처리합니다.
    /// </summary>
    //private IEnumerator DragLoop()
    //{
    //    while (isSwiping)
    //    {
    //        Vector2 curPos = InputManager.Instance.GetTouchWorldPosition();
    //        Vector2 dir = curPos - startPos;
    //        float distance = dir.magnitude;

    //        if (distance >= responsiveness)
    //        {
    //            dir = dir.normalized;
    //            float speedFactor = Mathf.Clamp(distance / 3, 0f, 1f);

    //            // 회전
    //            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
    //            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

    //            // 이동
    //            transform.position += (Vector3)dir * player.playerData.moveSpeed * speedFactor * Time.deltaTime;
    //        }

    //        yield return null;
    //    }
    //}
}
