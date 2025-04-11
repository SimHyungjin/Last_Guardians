using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    public TowerData towerdata;
    public GameObject towerGhostPrefab;
    public bool isMoving;
    public bool isCliked;

    private GameObject towerGhost;
    public SpriteRenderer sprite;
    Vector2 curPos;

    public void Init(int index)
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        towerdata = Resources.Load<TowerData>($"SO/Tower/Tower{index}");
        sprite = GetComponent<SpriteRenderer>();
        if (towerdata != null)
        {
            int spriteIndex = (towerdata.TowerIndex>49)? towerdata.TowerIndex-49: towerdata.TowerIndex;
            sprite.sprite = towerdata.atlas.GetSprite($"Tower_{spriteIndex}");
            towerGhost = towerdata.towerGhostPrefab;
        }
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
        if (!isCliked)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower"));
            if (hit != null && hit.gameObject == this.gameObject)
            {
                isCliked = true;
                TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
            }
        }
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (isCliked)
        {
            isCliked = false;
            TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
            if (TowerManager.Instance.towerbuilder.CanTowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition()))
            {
                TowerManager.Instance.towerbuilder.TowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition());
            }
        }
    }
    private void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        isMoving = false;
    }
}

