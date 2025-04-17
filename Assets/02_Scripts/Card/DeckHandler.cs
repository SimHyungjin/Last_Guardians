using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using static UnityEditor.PlayerSettings;

public class DeckHandler : MonoBehaviour
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
    public GameObject ghostTowerPrefab;
    private GameObject ghostTower;
    private float Deadzone = 80f;
    public bool IsHighlighting => isHighlighting;
    public Card HighlightedCard => highlightedCard;
    public int HighlightedIndex => highlightedIndex;    
    private void Update()
    {
        if (isDragging)
        {
            highlightedCard.transform.position = InputManager.Instance.GetTouchPosition();
            Vector2 dragEndPos = InputManager.Instance.GetTouchPosition();
            dragDistance = Vector2.Distance(dragStartPos, dragEndPos);
            if (dragDistance > Deadzone)
            {
                if (ghostTower == null)
                {
                    TowerManager.Instance.towerbuilder.ChangeCardMove();
                    highlightedCard.gameObject.SetActive(false);
                    ghostTower = Instantiate(ghostTowerPrefab, InputManager.Instance.GetTouchPosition(), Quaternion.identity);
                    SpriteRenderer ghostsprite = ghostTower.GetComponent<SpriteRenderer>();
                    TowerData towerData = TowerManager.Instance.GetTowerData(highlightedIndex);
                    int spriteIndex;
                    if (towerData.TowerIndex > 49 && towerData.TowerIndex < 99)
                    {
                        spriteIndex = towerData.TowerIndex - 49;
                    }
                    else if (towerData.TowerIndex > 98 && towerData.TowerIndex < 109)
                    {
                        spriteIndex = towerData.TowerIndex - 98;
                    }
                    else if (towerData.TowerIndex > 108)
                    {
                        spriteIndex = towerData.TowerIndex - 59;
                    }
                    else
                    {
                        spriteIndex = towerData.TowerIndex;
                    }
                    ghostsprite.sprite = towerData.atlas.GetSprite($"Tower_{spriteIndex}");
                    //towerGhost = towerData.towerGhostPrefab;


                }
                else
                {
                    ghostTower.transform.position = InputManager.Instance.GetTouchWorldPosition();
                }
            }
            else if(ghostTower != null)
            {
                TowerManager.Instance.towerbuilder.ChangeCardMove();
                highlightedCard.gameObject.SetActive(true);
                Destroy(ghostTower);
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

            isDragging = true;
            dragStartPos = InputManager.Instance.GetTouchPosition();
            dragDistance = 0;
        }
        else
        {
        }
    }

    public void MoveCardEnd(Card card)
    {
        TowerManager.Instance.towerbuilder.EndAttackRangeCircle();
        if (isHighlighting && card.TowerIndex == highlightedIndex)
        {

            if (dragDistance < Deadzone)
            {
                dragEndPos = InputManager.Instance.GetTouchPosition();
                UnHighlightCard();
            }
            else
            {
                TowerManager.Instance.towerbuilder.ChangeCardMove();
                Destroy(ghostTower);
                ghostTower = null;
                StartCoroutine(TowerManager.Instance.towerbuilder.CanConstructCoroutine(
                                InputManager.Instance.GetTouchWorldPosition(),
                                (canPlace) =>
                                {
                                    if (canPlace)
                                    {
                                        TowerManager.Instance.towerbuilder.TowerConstruct(
                                        InputManager.Instance.GetTouchWorldPosition(),
                                        highlightedIndex                                        
                                        );
                                        UseCard(); // 카드 사용 처리
                                    }
                                    else if (TowerManager.Instance.towerbuilder.CanCardToTowerCombine(InputManager.Instance.GetTouchWorldPosition(),highlightedIndex))
                                    {
                                        Debug.Log("합성시작");
                                        TowerManager.Instance.towerbuilder.CardToTowerCombine(InputManager.Instance.GetTouchWorldPosition());
                                        UseCard();
                                    }
                                    else
                                    {
                                        Debug.Log("건설 불가");
                                        highlightedCard.gameObject.SetActive(true);
                                        highlightedCard.transform.position = InputManager.Instance.GetTouchPosition();
                                        UnHighlightCard();
                                    }
                                    Destroy(ghostTower);
                                    ghostTower = null;
                                }));
            }
            isDragging = false;
        }
        else if(TowerManager.Instance.CanStartInteraction())
        {
            HighlightCard(card);
            TowerManager.Instance.StartInteraction(InteractionState.CardMoving);
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
            highlightedCard.onClicked -= MoveCardStart;
            highlightedCard.onClickEnd -= MoveCardEnd;
            RectTransform targetRect = cards[highlightedOrder].GetComponent<RectTransform>();
            Vector2 endPos = targetRect.anchoredPosition;

            rect.DOAnchorPos(endPos, 0.3f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
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
        isHighlighting = false;
        TowerManager.Instance.EndInteraction(InteractionState.CardMoving);
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
    public void UseCard()
    {
        RectTransform handRect = GetComponent<RectTransform>();
        handRect.DOAnchorPos(handRect.anchoredPosition + new Vector2(0, 100f), 0.3f).SetEase(Ease.OutCubic);
        highlightedCard.onClicked -= MoveCardStart;
        highlightedCard.onClickEnd -= MoveCardEnd;
        Destroy(highlightedCard.gameObject);
        ResetHighlightState();
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

}
