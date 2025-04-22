using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineVirtualCamera freeCam;
    [SerializeField] private CinemachineVirtualCamera focusCam;

    private CinemachineVirtualCamera currentCam;

    [SerializeField] private GameObject dummyObj;

    private bool onCamMove = false;
    private Coroutine moveCoroutine;

    private Vector2 lastTouchWorldPos;

    private void Start()
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        freeCam.Follow = dummyObj.transform;
        focusCam.Follow = dummyObj.transform;
    }
    private void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
#else
        if (Input.touchCount > 0 && EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif

        Vector2 curPos = InputManager.Instance.GetTouchWorldPosition();
        if (!Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Player"))&& !Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower")))
        {
            if (onCamMove) return;
            onCamMove = true;

            lastTouchWorldPos = curPos;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            moveCoroutine = StartCoroutine(MoveStart());
        }
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (!onCamMove) return;
        onCamMove = false;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    private IEnumerator MoveStart()
    {
        while (onCamMove)
        {
            yield return new WaitForEndOfFrame();

            Vector2 curTouchWorldPos = InputManager.Instance.GetTouchWorldPosition();
            Vector2 delta = curTouchWorldPos - lastTouchWorldPos;

            currentCam = freeCam.Priority > focusCam.Priority ? freeCam : focusCam;

            Vector3 newPos = dummyObj.transform.position - (Vector3)delta * 10;
            newPos = GetConfinedPosition(currentCam, newPos);

            dummyObj.transform.position = newPos;
            lastTouchWorldPos = curTouchWorldPos;
        }
    }

    private Vector3 GetConfinedPosition(CinemachineVirtualCamera cam, Vector3 desiredPos)
    {
        var confiner = cam.GetComponent<CinemachineConfiner2D>();
        if (confiner == null || confiner.m_BoundingShape2D == null) return desiredPos;

        float halfHeight = cam.m_Lens.OrthographicSize;
        float halfWidth = halfHeight * cam.m_Lens.Aspect;

        Bounds bounds = confiner.m_BoundingShape2D.bounds;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;
        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        float clampedX = Mathf.Clamp(desiredPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPos.y, minY, maxY);

        return new Vector3(clampedX, clampedY, desiredPos.z);
    }
}
