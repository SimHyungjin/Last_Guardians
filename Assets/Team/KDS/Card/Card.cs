using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] private int towerIndex;
    [SerializeField] private int stack;
    [SerializeField] private Sprite cardImage;

    public GameObject towerGhostPrefab;

    public event Action<Card> onClicked;
    public event Action<Card> onClickEnd;
    public TextMeshPro Text;
    private bool isMoving = false;
    Vector2 curPos;
    public int TowerIndex => towerIndex;
    public int Stack => stack;

    public void Start()
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
    }
    public void OnTouchStart(InputAction.CallbackContext ctx)
    {
        curPos = InputManager.Instance.GetTouchWorldPosition();
        Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Card"));
        if (hit != null && hit.gameObject == this.gameObject&& !isMoving)
        {
            isMoving = true;
            onClicked?.Invoke(this);
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

    public void MoveCardStart()
    {
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
