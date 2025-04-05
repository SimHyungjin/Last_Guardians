using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public MouseController mouseInput { get; private set; }
    public MouseController.MouseActions mouseActions { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        mouseInput = new MouseController();
        mouseActions = mouseInput.Mouse;
        mouseInput.Enable();
    }

    public Vector2 GetTouchWorldPosition()
    {
        Vector3 screenPos = mouseInput.Mouse.Position.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }

    public void BindTouch(Action<InputAction.CallbackContext> onStart, Action<InputAction.CallbackContext> onEnd)
    {
        mouseActions.Pressed.started += onStart;
        mouseActions.Pressed.canceled += onEnd;
    }

    public void BindStart(InputAction action, Action<InputAction.CallbackContext> callback) => action.started += callback;
    public void BindPerformed(InputAction action, Action<InputAction.CallbackContext> callback) => action.performed += callback;
    public void BindCanceled(InputAction action, Action<InputAction.CallbackContext> callback) => action.canceled += callback;

    public void UnBindStart(InputAction action, Action<InputAction.CallbackContext> callback) => action.started -= callback;
    public void UnBindPerformed(InputAction action, Action<InputAction.CallbackContext> callback) => action.performed -= callback;
    public void UnBindCanceled(InputAction action, Action<InputAction.CallbackContext> callback) => action.canceled -= callback;

    public void TouchEnable()
    {
        mouseInput.Enable();
    }
    public void TouchDisable()
    {
        mouseInput.Disable();
    }
}
