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
    private bool isSwiping = false;

    private Coroutine dragCoroutine;

    // TODO: UI가 완성되기 전까지 사용하는 테스트용 시각화 오브젝트
    private GameObject testObj;

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

        // TODO: 터치 시작 지점에 테스트 오브젝트 표시
        testObj = new GameObject("MouseTestObj");
        var sr = testObj.AddComponent<SpriteRenderer>();
        sr.color = Color.white;
        var sprite = Resources.Load<Sprite>("MouseTest");
        if (sprite != null)
            sr.sprite = sprite;
        else
            Debug.LogWarning("MouseTest 스프라이트를 찾을 수 없습니다.");
        testObj.transform.position = startPos;
        // TODO: 끝

        dragCoroutine = StartCoroutine(DragLoop());
    }

    /// <summary>
    /// 드래그 입력이 끝날 때 호출됩니다.
    /// 정보를 초기화합니다.
    /// </summary>
    public void SwipeStop()
    {
        if (!isSwiping) return;

        isSwiping = false;

        if (dragCoroutine != null)
        {
            // test용: 시각화 오브젝트 제거
            if (testObj != null) Destroy(testObj);

            StopCoroutine(dragCoroutine);
            dragCoroutine = null;
        }
    }

    /// <summary>
    /// 드래그 방향을 받아 캐릭터를 이동시키는 루프입니다.
    /// 일정 거리 이상일 경우 이동하며, 회전도 함께 처리합니다.
    /// </summary>
    private IEnumerator DragLoop()
    {
        while (isSwiping)
        {
            Vector2 curPos = InputManager.Instance.GetTouchWorldPosition();
            Vector2 dir = curPos - startPos;
            float distance = dir.magnitude;

            if (distance >= responsiveness)
            {
                dir = dir.normalized;
                float speedFactor = Mathf.Clamp(distance / 3, 0f, 1f);

                // 회전
                float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

                // 이동
                transform.position += (Vector3)dir * player.playerData.moveSpeed * speedFactor * Time.deltaTime;
            }

            yield return null;
        }
    }
}
