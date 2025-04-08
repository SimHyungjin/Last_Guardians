using DG.Tweening;
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
    [SerializeField] private Image cardImage;


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
        //나중에 리소스로 옮길것
        //cardData = Resources.Load<CardData>($"ScriptalbeObject/CardData{index}");
        string path = $"Assets/Team/KDS/ScriptableObject/Card/CardData{index}.asset";
        cardData = AssetDatabase.LoadAssetAtPath<CardData>(path);
        cardImage = GetComponent<Image>();
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
        if (!ctx.started) return;
        if (DeckManager.Instance.hand.IsHighlighting &&
            DeckManager.Instance.hand.HighlightedCard != this)
        {
            return;
        }
        //if (!gameObject.activeInHierarchy) return;
        screenPos = InputManager.Instance.GetTouchPosition();
        // UI용 레이캐스트
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
