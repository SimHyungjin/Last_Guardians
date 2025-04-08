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

    private bool isHighlighting = false;
    private int highlightedIndex = -1;
    private int highlightedOrder = -1;
    [SerializeField]private Card highlightedCard = null;

    public bool IsHighlighting => isHighlighting;
    public Card HighlightedCard => highlightedCard;
    void UpdateLayout()
    {
        int count = cards.Count;
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            (Vector2 pos, float angle) = GetCardLayout(i, count);

            RectTransform rect = cards[i].GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.DOAnchorPos(pos, 0.5f).SetEase(Ease.OutCubic);
                rect.DOLocalRotate(new Vector3(0, 0, angle), 0.5f).SetEase(Ease.OutCubic);
                rect.SetSiblingIndex(i);
            }

            SpriteRenderer sr = cards[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 100 + i;
            }
        }
    }

    public void MoveCardStart(Card card)
    {
        if (isHighlighting)
        {
            Debug.Log("하이라이트 중인 카드 클릭 " + card.TowerIndex);
        }
        else
        {
            Debug.Log("Card Clicked: " + card.TowerIndex);
        }
    }

    public void MoveCardEnd(Card card)
    {
        if(isHighlighting)
        {
            Debug.Log("하이라이트 중인 카드 클릭 해제 " + card.TowerIndex);
        }
        else
        {

            HighlightCard(card);
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
        (Vector2 targetPos, float angle) = GetCardLayout(cardIndex, total);

        cardRect.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutCubic);
        cardRect.DOLocalRotate(new Vector3(0, 0, angle), 0.5f).SetEase(Ease.OutCubic);
        cardRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
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

       

        rect.SetAsLastSibling();

        Vector2 centerBottom = new Vector2(0f, 650f);
        rect.DOAnchorPos(centerBottom, 0.3f).SetEase(Ease.OutCubic);
        rect.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutCubic);
        rect.DOScale(1.2f * Vector3.one, 0.3f).SetEase(Ease.OutBack);

        RectTransform handRect = GetComponent<RectTransform>();
        handRect.DOAnchorPos(handRect.anchoredPosition - new Vector2(0, 100f), 0.3f).SetEase(Ease.OutCubic);

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

    (Vector2 position, float angle) GetCardLayout(int index, int totalCount)
    {
        float dynamicMaxAngle = Mathf.Min(9f * (totalCount - 1), 36f);
        float angleStep = (totalCount > 1) ? (dynamicMaxAngle * 2) / (totalCount - 1) : 0f;

        float angle = dynamicMaxAngle - angleStep * index;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 pos = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * radius;
        return (pos, angle);
    }
}
