using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// 이 스크립트는 캐릭터의 입력 처리를 담당합니다.
/// 터치 또는 마우스 입력을 받아 캐릭터 이동 컨트롤러에 연결합니다.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private bool isJoystickMode = false;
    private JoystickUIController joystickUI;
    private PlayerMoveController moveController;

    private InputManager inputManager;

    private void Awake()
    {
        moveController = GetComponent<PlayerMoveController>();
        inputManager = InputManager.Instance;
        joystickUI = InGameManager.Instance.joystickUIController;

        joystickUI.OnDirectionTick += OnJoysticMove;
        joystickUI.ChangeState += SetInputMode;
    }

    /// <summary>
    /// 입력 시스템과 연결합니다.
    /// </summary>
    public void Init()
    {
        inputManager.BindTouchPressed(OnTouchStart, OnTouchEnd);
    }

    private void OnDestroy()
    {
        inputManager.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        joystickUI.OnDirectionTick += OnJoysticMove;
        joystickUI.ChangeState -= SetInputMode;
    }

    /// <summary>
    /// UI 위가 아니라면 캐릭터 이동 시작을 호출합니다.
    /// </summary>
    /// <param name="ctx">입력 컨텍스트</param>
    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        if(!IsPointerOverUI() && !isJoystickMode)
        {
            Vector2 curPos = inputManager.GetTouchWorldPosition();
            var hits = Physics2D.OverlapPointAll(curPos, LayerMask.GetMask("Player"));
            if (hits.Length > 0)
            {
                moveController.SwipeStart();
            }
        }
    }
    private void OnJoysticMove(Vector2 inputDir)
    {
        if(isJoystickMode)
            moveController.SetJoystickInput(inputDir);
    }
    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        moveController.SwipeStop();
    }

    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        return EventSystem.current.IsPointerOverGameObject();
#else
        return Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }

    public void SetInputMode(bool useJoystick)
    {
        isJoystickMode = useJoystick;
    }
}
