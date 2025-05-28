using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickUIController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBase;
    [SerializeField] private RectTransform joystickHandle;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float repeatRate = 0.02f;

    private PlayerInputStyle playerInputStyle;
    public Action<Vector2> OnDirectionTick;
    public Action<PlayerInputStyle> ChangeStyle;

    private Vector2 currentDirection = Vector2.zero;
    private Coroutine moveLoop;

    private void Awake()
    {
        playerInputStyle = GameManager.Instance.PlayerManager.playerInputStyle;
        SetVisible(playerInputStyle);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateHandlePosition(eventData);
        StartMoveLoop();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateHandlePosition(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickHandle.anchoredPosition = Vector2.zero;
        StopMoveLoop();
        OnDirectionTick?.Invoke(Vector2.zero);
    }

    private void UpdateHandlePosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        joystickBase,
        eventData.position,
        eventData.pressEventCamera,
        out localPoint
        );

        Vector2 direction = localPoint.normalized;
        float distance = Mathf.Min(localPoint.magnitude, maxDistance);

        joystickHandle.anchoredPosition = direction * distance;
        currentDirection = direction;
    }

    private void StartMoveLoop()
    {
        if (moveLoop != null) return;
        moveLoop = StartCoroutine(MoveLoop());
    }

    private void StopMoveLoop()
    {
        if (moveLoop != null)
        {
            StopCoroutine(moveLoop);
            moveLoop = null;
        }
    }

    private IEnumerator MoveLoop()
    {
        while (true)
        {
            if (currentDirection.sqrMagnitude > 0.01f)
                OnDirectionTick?.Invoke(currentDirection);
            else
                OnDirectionTick?.Invoke(Vector2.zero);

            yield return new WaitForSeconds(repeatRate);
        }
    }

    public void SetVisible(PlayerInputStyle style)
    {
        playerInputStyle = style;
        if (playerInputStyle == PlayerInputStyle.Swipe)
        {
            gameObject.SetActive(false);
        }
        else if (playerInputStyle == PlayerInputStyle.Joystick)
        {
            gameObject.SetActive(true);
        }
        ChangeStyle?.Invoke(playerInputStyle);
    }
}
