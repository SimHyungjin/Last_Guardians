using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    public TowerData towerdata;
    public GameObject towerGhost;
    public bool isMoving;

    private SpriteRenderer sprite;
    Vector2 curPos;

    public void Init(int index)
    {
        Debug.Log("Init");
        string path = $"Assets/Team/KDS/ScriptableObject/Tower/TestTower{index}.asset";
        towerdata = AssetDatabase.LoadAssetAtPath<TowerData>(path);
        Debug.Log(towerdata.TowerName);
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        //나중에 리소스로 옮길것
        //towerdata = Resources.Load<CardData>($"ScriptalbeObject/Towerdata{index}");
        sprite = GetComponent<SpriteRenderer>();
        if (towerdata != null)
        {
            sprite.sprite = towerdata.towerSprite;
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
        towerGhost.GetComponent<SpriteRenderer>().sprite = towerdata.towerSprite;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
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
                Tower targetTower = collider.GetComponent<Tower>();
                //TowerManager.Instance.towerConstructer.TowerCombine(this, targetTower);
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
                Destroy(towerGhost);
                return;
            }
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        Destroy(towerGhost);
        towerGhost = null;
    }
    private void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        isMoving = false;
    }
}

