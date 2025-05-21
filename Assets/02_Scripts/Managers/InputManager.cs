using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// 터치 입력을 관리하는 매니저입니다.
/// </summary>
public class InputManager : Singleton<InputManager>
{
    private PointerInput pointerInput;
    private PointerInput.PointerActions pointerAction;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pointerInput = new PointerInput();
        pointerAction = pointerInput.Pointer;
        pointerInput.Enable();
    }

    /// <summary>
    /// 터치 입력을 통해 월드 좌표를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetTouchWorldPosition()
    {
        Vector3 screenPos = pointerInput.Pointer.Position.ReadValue<Vector2>();
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }
    /// <summary>
    /// 터치 입력을 통해 스크린 좌표를 가져옵니다.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetTouchPosition()
    {
        return pointerInput.Pointer.Position.ReadValue<Vector2>();
    }
    /// <summary>
    /// UI 위에 터치가 있는지 확인합니다. 터치 입력이 없거나 UI 위에 터치가 없다면 false를 반환합니다.
    /// </summary>
    /// <returns></returns>

    public bool IsTouchOverUI()
    {
#if UNITY_EDITOR
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

    if (EventSystem.current == null)
        return false;
#else
        PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = pointerInput.Pointer.Position.ReadValue<Vector2>();

    var results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, results);
    return results.Count > 0;
#endif
    }
    /// <summary>
    /// 터치를 시작할 때와 끝날 때의 콜백을 바인딩합니다.
    /// </summary>
    /// <param name="onStart"></param>
    /// <param name="onEnd"></param>
    public void BindTouchPressed(Action<InputAction.CallbackContext> onStart, Action<InputAction.CallbackContext> onEnd)
    {
        pointerAction.Pressed.started += onStart;
        pointerAction.Pressed.canceled += onEnd;
    }
    /// <summary>
    /// 터치 입력을 시작할 때와 끝날 때의 콜백을 언바인딩합니다.
    /// </summary>
    /// <param name="onStart"></param>
    /// <param name="onEnd"></param>
    public void UnBindTouchPressed(Action<InputAction.CallbackContext> onStart, Action<InputAction.CallbackContext> onEnd)
    {
        pointerAction.Pressed.started -= onStart;
        pointerAction.Pressed.canceled -= onEnd;
    }
    public void BindStart(InputAction action, Action<InputAction.CallbackContext> callback) => action.started += callback;
    public void BindPerformed(InputAction action, Action<InputAction.CallbackContext> callback) => action.performed += callback;
    public void BindCanceled(InputAction action, Action<InputAction.CallbackContext> callback) => action.canceled += callback;

    public void UnBindStart(InputAction action, Action<InputAction.CallbackContext> callback) => action.started -= callback;
    public void UnBindPerformed(InputAction action, Action<InputAction.CallbackContext> callback) => action.performed -= callback;
    public void UnBindCanceled(InputAction action, Action<InputAction.CallbackContext> callback) => action.canceled -= callback;

    public void EnablePointer()
    {
        pointerInput.Enable();
    }
    public void DisablePointer()
    {
        pointerInput.Disable();
    }
}
