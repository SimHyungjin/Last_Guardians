using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    public TowerData towerdata;
    public GameObject towerGhost;

    private void OnEnable()
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
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

        if (Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower")))
            MoveTowerStart();
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
    }
    public void MoveTowerStart()
    {
        Debug.Log("MoveTowerStart");
    }
}
