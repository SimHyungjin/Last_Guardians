using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PointerInput pointerInput;
    private PointerInput.PointerActions pointerAction;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        pointerInput = new PointerInput();
        pointerAction = pointerInput.Pointer;
        pointerInput.Enable();
    }

    public Vector2 GetTouchWorldPosition()
    {
        Vector3 screenPos = pointerInput.Pointer.Position.ReadValue<Vector2>();
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }

    public void BindTouchPressed(Action<InputAction.CallbackContext> onStart, Action<InputAction.CallbackContext> onEnd)
    {
        pointerAction.Pressed.started += onStart;
        pointerAction.Pressed.canceled += onEnd;
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
