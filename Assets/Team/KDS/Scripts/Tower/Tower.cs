using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    public TowerData towerdata;
    public GameObject towerGhost;
    public bool isMoving;

    private SpriteRenderer color;
    Vector2 curPos;
    private void OnEnable()
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        color = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isMoving)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            towerGhost.transform.position = curPos;
        }
    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
#else
    if (Input.touchCount > 0 && EventSystem.current != null &&
        EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
#endif

        curPos = InputManager.Instance.GetTouchWorldPosition();

        Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower"));
        if (hit != null && hit.gameObject == this.gameObject)
        {
            MoveTowerStart();
        }
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (isMoving)
        {
            isMoving = false;
            MoveTowerEnd();
        }
    }
    public void MoveTowerStart()
    {
        isMoving = true;
        towerGhost = Instantiate(towerdata.towerGhostPrefab, curPos, Quaternion.identity);
        color.color = new Color(color.color.r, color.color.g, color.color.b, 0.3f);
    }
    public void MoveTowerEnd()
    {
        curPos = InputManager.Instance.GetTouchWorldPosition();
        Collider2D currentCollider = GetComponent<Collider2D>();
        Collider2D[] colliders = Physics2D.OverlapPointAll(curPos, LayerMask.GetMask("Tower"));
        foreach (Collider2D collider in colliders)
        {
            if (collider != currentCollider) 
            {
                color.color = new Color(color.color.r, color.color.g, color.color.b, 1f);
                Destroy(towerGhost);
                return;
            }
        }
        color.color = new Color(color.color.r, color.color.g, color.color.b, 1f);
        Destroy(towerGhost);
        towerGhost = null;
    }
}

