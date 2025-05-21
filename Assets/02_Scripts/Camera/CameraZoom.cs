using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineConfiner2D freeCamBoundary;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 6f;
    [SerializeField] private Camera mainCamera;

    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();

    private void Awake()
    {
        mainCamera = Camera.main;
        freeCamBoundary = cinemachineVirtualCamera.GetComponent<CinemachineConfiner2D>();
    }

    void Update()
    {
        if (cinemachineVirtualCamera == null || mainCamera == null) return;

        var lens = cinemachineVirtualCamera.m_Lens;

        if (Touch.activeTouches.Count == 2)
        {
            var touch0 = Touch.activeTouches[0];
            var touch1 = Touch.activeTouches[1];

            if (IsPointerOverUI(touch0) || IsPointerOverUI(touch1)) return;

            Vector2 prevTouch0Pos = touch0.screenPosition - touch0.delta;
            Vector2 prevTouch1Pos = touch1.screenPosition - touch1.delta;

            float prevMagnitude = (prevTouch0Pos - prevTouch1Pos).magnitude;
            float currentMagnitude = (touch0.screenPosition - touch1.screenPosition).magnitude;

            float deltaMagnitude = currentMagnitude - prevMagnitude;
            Vector2 midPointNow = (touch0.screenPosition + touch1.screenPosition) * 0.5f;

            ZoomAtScreenPosition(midPointNow, deltaMagnitude);
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        float scrollDelta = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ZoomAtScreenPosition(mousePos, scrollDelta * 0.1f);
        }
#endif
    }

    private void ZoomAtScreenPosition(Vector2 screenPosition, float deltaMagnitude)
    {
        var lens = cinemachineVirtualCamera.m_Lens;
        float targetSize = Mathf.Clamp(lens.OrthographicSize - deltaMagnitude * zoomSpeed, minZoom, maxZoom);
        if (Mathf.Approximately(lens.OrthographicSize, targetSize)) return;

        float fixedZ = mainCamera.transform.position.z;
        Vector3 screenPosWithZ = new Vector3(screenPosition.x, screenPosition.y, -fixedZ);
        Vector3 worldBefore = mainCamera.ScreenToWorldPoint(screenPosWithZ);

        lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, targetSize, Time.deltaTime * zoomSpeed * 20f);
        cinemachineVirtualCamera.m_Lens = lens;

        Vector3 worldAfter = mainCamera.ScreenToWorldPoint(screenPosWithZ);
        Vector3 delta = worldBefore - worldAfter;

        Vector3 targetPos = cinemachineVirtualCamera.transform.position + delta;

        targetPos.z = cinemachineVirtualCamera.transform.position.z;

        targetPos = GetConfinedPosition(targetPos);

        cinemachineVirtualCamera.transform.position = Vector3.Lerp(cinemachineVirtualCamera.transform.position, targetPos, Time.deltaTime * 10f);

        if (cinemachineVirtualCamera.Follow != null)
            cinemachineVirtualCamera.Follow.position = new Vector3(
                cinemachineVirtualCamera.transform.position.x,
                cinemachineVirtualCamera.transform.position.y,
                cinemachineVirtualCamera.Follow.position.z
            );

        freeCamBoundary.InvalidateCache();
    }

    private Vector3 GetConfinedPosition(Vector3 desiredPos)
    {
        if (freeCamBoundary == null || freeCamBoundary.m_BoundingShape2D == null) return desiredPos;

        float halfHeight = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float halfWidth = halfHeight * cinemachineVirtualCamera.m_Lens.Aspect;

        Bounds bounds = freeCamBoundary.m_BoundingShape2D.bounds;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;
        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        float clampedX = Mathf.Clamp(desiredPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPos.y, minY, maxY);
        float fixedZ = cinemachineVirtualCamera.transform.position.z;

        return new Vector3(clampedX, clampedY, fixedZ);
    }

    private bool IsPointerOverUI(Touch touch)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.finger.index);
    }
}
