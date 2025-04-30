using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;
[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] private int towerIndex;
    [SerializeField] private int stack;
    [SerializeField] private SpriteAtlas atlas;


    public GameObject towerGhostPrefab;
    

    public event Action<Card> onClicked;
    public event Action<Card> onClickEnd;
    public TextMeshProUGUI Text;

    private bool isMoving = false;

    Vector2 screenPos;
    Vector2 curPos;
    public int TowerIndex => towerIndex;
    public int Stack => stack;

    public void Init(int index)
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        towerIndex = index;
        GetComponent<Image>().sprite = atlas.GetSprite($"Card_{index}");
        stack = 1;
        ShowStack();
    }
    /// <summary>
    /// 카드에 클릭이 들어왔을때
    /// 덱핸들러에게 메세지를 보낸다.
    /// </summary>
    /// <param name="ctx"></param>
    public void OnTouchStart(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if (TowerManager.Instance.hand.IsHighlighting &&
            TowerManager.Instance.hand.HighlightedCard != this)
        {
            return;
        }
        screenPos = InputManager.Instance.GetTouchPosition();
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPos;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        GraphicRaycaster raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject == this.gameObject && !isMoving)
            {
                isMoving = true;
                onClicked?.Invoke(this);
                break;
            }
        }
    }
    public void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (isMoving)
        {
            isMoving = false;
            onClickEnd?.Invoke(this);
        }
    }

    public void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        onClicked = null;
        onClickEnd = null;
    }

    public void ShowStack()
    {
        if (stack > 1)
        {
            Text.text = "X"+stack.ToString();
        }
        else
        {
            Text.text = "";
        }
    }
    public void AddStack()
    {
        stack++;
    }
    
    public void subtractStack()
    {
        stack--;
    }   
}
