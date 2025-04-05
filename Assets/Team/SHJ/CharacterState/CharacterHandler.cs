using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterHandler : MonoBehaviour
{
    private CharacterController characterController;

    public void Init(CharacterController _characterController)
    {
        characterController = InGameManager.Instance.CharacterManager.characterController;
        InputManager.Instance.BindTouch(OnTouchStart, OnTouchEnd);
    }

    void OnTouchStart(InputAction.CallbackContext ctx)
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
#else
    if (Input.touchCount > 0 && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif
        characterController.SwipeStart();
    }

    void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        characterController.SwipeStop();
    }
}
