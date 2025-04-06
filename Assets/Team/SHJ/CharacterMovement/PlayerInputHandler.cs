using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// 이 스크립트는 캐릭터의 입력 처리를 담당합니다.
/// 터치 또는 마우스 입력을 받아 캐릭터 이동 컨트롤러에 연결합니다.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController playercontroller;

    /// <summary>
    /// 컴포넌트 참조를 초기화합니다.
    /// </summary>
    private void Awake()
    {
        playercontroller = GetComponent<PlayerController>();
    }

    /// <summary>
    /// 입력 시스템과 연결합니다.
    /// </summary>
    public void Init()
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
    }

    /// <summary>
    /// 터치 또는 클릭 시작 시 호출됩니다. UI 위가 아니라면 캐릭터 이동 시작을 호출합니다.
    /// </summary>
    /// <param name="ctx">입력 컨텍스트</param>
    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
#else
        if (Input.touchCount > 0 && EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif
        playercontroller.moveController.SwipeStart();
    }

    /// <summary>
    /// 터치 또는 클릭 종료 시 호출됩니다. 캐릭터 이동을 멈춥니다.
    /// </summary>
    /// <param name="ctx">입력 컨텍스트</param>
    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        playercontroller.moveController.SwipeStop();
    }
}
