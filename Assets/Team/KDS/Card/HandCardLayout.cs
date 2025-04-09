using DG.Tweening;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class HandCardLayout : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    [SerializeField] private Card cardPrefab;
    private float radius = 500f;
    public float maxAngle = 30f;
    public Vector2 dragStartPos;
    public Vector2 dragEndPos;
    float dragDistance;

    [SerializeField] private Card highlightedCard = null;
    private bool isHighlighting = false;
    private int highlightedIndex = -1;
    private int highlightedOrder = -1;
    private int originalSiblingIndex;
    private bool isDragging = false;

    private GameObject ghostTower;
    public bool IsHighlighting => isHighlighting;
    public Card HighlightedCard => highlightedCard;

    private void Update()
    {
        if (isDragging)
        {
            highlightedCard.transform.position = InputManager.Instance.GetTouchPosition();
            Vector2 dragEndPos = InputManager.Instance.GetTouchPosition();
            dragDistance = Vector2.Distance(dragStartPos, dragEndPos);
            if (dragDistance > 200f)
            {
                if (ghostTower == null)
                {
                    highlightedCard.gameObject.SetActive(false);

                    ghostTower = Instantiate(highlightedCard.towerGhostPrefab, InputManager.Instance.GetTouchPosition(), Quaternion.identity);
                }
                else
                {
                    ghostTower.transform.position = InputManager.Instance.GetTouchWorldPosition();
                }
            }
            else
            {
            }
        }
    }
    
    private void UpdateLayout()
    {
        int count = cards.Count;
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            RectTransform rect = cards[i].GetComponent<RectTransform>();
            GetCardLayout(i, count, rect);
            SpriteRenderer sr = cards[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 100 + i;
            }
        }
    }

    public void MoveCardStart(Card card)
    {
        if (isHighlighting&&card.TowerIndex==highlightedIndex)
        {
            Debug.Log("하이라이트 중인 카드 클릭 " + card.TowerIndex);

            isDragging = true;
            dragStartPos = InputManager.Instance.GetTouchPosition();
            dragDistance = 0;
        }
        else
        {
            Debug.Log("Card Clicked: " + card.TowerIndex);
        }
    }

    public void MoveCardEnd(Card card)
    {
        if(isHighlighting && card.TowerIndex == highlightedIndex)
        {

            if (dragDistance < 200f)
            {
                Debug.Log("하이라이트해제~"+ card.TowerIndex);
                dragEndPos = InputManager.Instance.GetTouchPosition();
                UnHighlightCard();
            }
            else 
            { 
                Destroy(ghostTower);
                ghostTower = null;
                highlightedCard.gameObject.SetActive(true);
                highlightedCard.transform.position = InputManager.Instance.GetTouchPosition();
                UnHighlightCard();
            }
            isHighlighting = false;
            isDragging = false;
        }
        else
        {
            HighlightCard(card);
            Debug.Log("하이라이트시작: " + card.TowerIndex);
            Debug.Log("Card Clicked End: " + card.TowerIndex);
        }
    }

    public void AddCard(int index)
    {
        foreach (Card card in cards)
        {
            if (card.TowerIndex == index)
            {
                card.AddStack();
                card.ShowStack();
                return;
            }
        }

        Card addCard = Instantiate(cardPrefab, transform);
        addCard.Init(index);
        addCard.onClicked += MoveCardStart;
        addCard.onClickEnd += MoveCardEnd;
        cards.Add(addCard);
        UpdateLayout();
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 localStartPos = transform.InverseTransformPoint(screenCenter);
        RectTransform cardRect = addCard.GetComponent<RectTransform>();
        cardRect.localPosition = localStartPos;
        cardRect.localScale = Vector3.one * 1.2f;

        (int cardIndex, int total) = (cards.Count - 1, cards.Count);
        GetCardLayout(cardIndex, total, cardRect);
    }

    public void HighlightCard(Card card)
    {
        if (isHighlighting) return;
        isHighlighting = true;

        highlightedIndex = card.TowerIndex;
        highlightedOrder = cards.IndexOf(card);
        highlightedCard = card;
        RectTransform rect;
        if (card.Stack == 1)
        {
            cards.Remove(card);
            UpdateLayout();
            rect = card.GetComponent<RectTransform>();
        }
        else 
        {
            card.subtractStack();
            card.ShowStack();
            Card highlightClone = Instantiate(cardPrefab, transform);
            highlightClone.Init(card.TowerIndex);
            highlightClone.onClicked += MoveCardStart;
            highlightClone.onClickEnd += MoveCardEnd;
            highlightClone.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
            highlightedCard = highlightClone;
            rect = highlightClone.GetComponent<RectTransform>();
        }
        originalSiblingIndex = card.transform.GetSiblingIndex();
        rect.SetAsLastSibling();
        Vector2 centerBottom = new Vector2(0f, 650f);
        rect.DOAnchorPos(centerBottom, 0.3f).SetEase(Ease.OutCubic);
        rect.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutCubic);
        rect.DOScale(1.2f * Vector3.one, 0.3f).SetEase(Ease.OutBack);

        RectTransform handRect = GetComponent<RectTransform>();
        handRect.DOAnchorPos(handRect.anchoredPosition - new Vector2(0, 100f), 0.3f).SetEase(Ease.OutCubic);
    }

    public void UnHighlightCard()
    {
        RectTransform handRect = GetComponent<RectTransform>();
        handRect.DOAnchorPos(handRect.anchoredPosition + new Vector2(0, 100f), 0.3f).SetEase(Ease.OutCubic);
        bool stackExists=false;
        foreach (Card card in cards)
        {
            if (card.TowerIndex == highlightedIndex)
            {
                stackExists = true;
                break;
            }
        }

        RectTransform rect = highlightedCard.GetComponent<RectTransform>();
        rect.SetSiblingIndex(originalSiblingIndex);
        if (stackExists)
        {
            RectTransform targetRect = cards[highlightedOrder].GetComponent<RectTransform>();
            Vector2 endPos = targetRect.anchoredPosition;

            rect.DOAnchorPos(endPos, 0.3f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                highlightedCard.onClicked -= MoveCardStart;
                highlightedCard.onClickEnd -= MoveCardEnd;
                cards[highlightedOrder].AddStack();
                cards[highlightedOrder].ShowStack();
                Destroy(highlightedCard.gameObject);
                ResetHighlightState();
            });
        }
        else
        {
            cards.Insert(highlightedOrder, highlightedCard);
            UpdateLayout();
            ResetHighlightState();
        }
    }

    private void ResetHighlightState()
    {
        highlightedCard = null;
        highlightedIndex = -1;
        highlightedOrder = -1;
    }

    public void UseCard(int index)
    {
        foreach (Card card in cards)
        {
            if (card.TowerIndex == index && card.Stack > 1)
            {
                card.subtractStack();
                card.ShowStack();
                return;
            }
            else if (card.TowerIndex == index && card.Stack == 1)
            {

                card.onClicked -= MoveCardStart;
                card.onClickEnd -= MoveCardEnd;
                card.transform.DOKill();
                cards.Remove(card);
                Destroy(card.gameObject);
                UpdateLayout();
                return;
            }
        }
    }

    private void GetCardLayout(int index, int totalCount,RectTransform rect)
    {
        float dynamicMaxAngle = Mathf.Min(9f * (totalCount - 1), 36f);
        float angleStep = (totalCount > 1) ? (dynamicMaxAngle * 2) / (totalCount - 1) : 0f;

        float angle = dynamicMaxAngle - angleStep * index;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 pos = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
        rect.DOAnchorPos(pos, 0.5f).SetEase(Ease.OutCubic);
        rect.DOLocalRotate(new Vector3(0, 0, angle), 0.5f).SetEase(Ease.OutCubic);
        rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    //(Vector2 position, float angle) GetCardLayout(int index, int totalCount, RectTransform rect)
    //{
    //    float dynamicMaxAngle = Mathf.Min(9f * (totalCount - 1), 36f);
    //    float angleStep = (totalCount > 1) ? (dynamicMaxAngle * 2) / (totalCount - 1) : 0f;

    //    float angle = dynamicMaxAngle - angleStep * index;
    //    float rad = angle * Mathf.Deg2Rad;
    //    Vector2 pos = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * radius;

    //    return (pos, angle);
    //}
}
