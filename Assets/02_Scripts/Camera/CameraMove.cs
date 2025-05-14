using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineVirtualCamera freeCam;
    [SerializeField] private GameObject dummyObj;

    private CinemachineConfiner2D freeCamBoundary;
    private Vector2 lastTouchWorldPos;
    private bool onCamMove = false;
    private Coroutine moveCoroutine;

    private void Start()
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        freeCam.Follow = dummyObj.transform;
        freeCamBoundary = freeCam.GetComponent<CinemachineConfiner2D>();
    }

    private void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        //if (Input.touchCount == 0) return;
        if (InputManager.Instance.IsTouchOverUI()) return;

        Vector2 curPos = InputManager.Instance.GetTouchWorldPosition();
        if (!Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Player")) && !Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower"))&& TowerManager.Instance.CanStartInteraction())
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

            float zoomScale = freeCam.m_Lens.OrthographicSize;
            Vector3 newPos = dummyObj.transform.position - (Vector3)delta * zoomScale;
            newPos = GetConfinedPosition(freeCam, newPos);

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

        float clampedX = (halfWidth * 2f > bounds.size.x) ? bounds.center.x : Mathf.Clamp(desiredPos.x, minX, maxX);
        float clampedY = (halfHeight * 2f > bounds.size.y) ? bounds.center.y : Mathf.Clamp(desiredPos.y, minY, maxY);

        return new Vector3(clampedX, clampedY, desiredPos.z);
    }

    public void FocusOnPlayer(Vector3 playerPosition)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        StartCoroutine(LerpToPosition(playerPosition));
    }

    private IEnumerator LerpToPosition(Vector3 targetPos)
    {
        Vector3 startPos = dummyObj.transform.position;
        targetPos.z = startPos.z;

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            dummyObj.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        dummyObj.transform.position = targetPos;
        freeCamBoundary.InvalidateCache();
    }
}
public static class VectorExtensions
{
    public static Vector3 WithZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }
}
