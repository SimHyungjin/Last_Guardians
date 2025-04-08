using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] public CardData cardData;
    [SerializeField] private int towerIndex;
    [SerializeField] private int stack;
    [SerializeField] private SpriteRenderer cardImage;

    public GameObject towerGhostPrefab;

    public event Action<Card> onClicked;
    public event Action<Card> onClickEnd;
    public TextMeshPro Text;
    private bool isMoving = false;
    Vector2 curPos;
    public int TowerIndex => towerIndex;
    public int Stack => stack;

    public void OnEnable()
    {
        Init(2);
    }
    public void Init(int index)
    {

        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        //나중에 리소스로 옮길것
        //cardData = Resources.Load<CardData>($"ScriptalbeObject/CardData{index}");
        string path = $"Assets/Team/KDS/ScriptableObject/Card/CardData{index}.asset";
        cardData = AssetDatabase.LoadAssetAtPath<CardData>(path);
        cardImage = GetComponent<SpriteRenderer>();
        if (cardData != null)
        {
            towerIndex = cardData.TowerIndex;
            cardImage.sprite= cardData.CardImage;
            towerGhostPrefab = cardData.towerGhostPrefab;
        }
        stack = 1;
        ShowStack();
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
