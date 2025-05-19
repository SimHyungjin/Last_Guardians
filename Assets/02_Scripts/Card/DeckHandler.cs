using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DeckHandler : MonoBehaviour
{
    [Header("Deck")]
    public List<Card> cards = new List<Card>();
    [SerializeField] private Card cardPrefab;
    private float radius = 500f;
    public float maxAngle = 30f;
    public Vector2 dragStartPos;
    public Vector2 dragEndPos;
    float dragDistance;

    [Header("HighLight")]
    [SerializeField] private Card highlightedCard = null;
    private bool isHighlighting = false;
    private int highlightedIndex = -1;
    private int highlightedOrder = -1;
    private int originalSiblingIndex;
    private Vector2 originalPosition;
    private bool isDragging = false;
    public GameObject ghostTowerPrefab;
    private GameObject ghostTower;
    private float Deadzone = 80f;
    public bool IsHighlighting => isHighlighting;
    public Card HighlightedCard => highlightedCard;
    public int HighlightedIndex => highlightedIndex;

    private void Awake()
    {
        originalPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    ///////////==========================카드 이동================================/////////////////////

    /// <summary>
    /// 클릭이후 클릭된 카드가 손에서 멀어지면 타워설치
    /// 안멀어지고 클릭이 취소되면 다시 손으로 들어가는 연출
    /// </summary>
    private void Update()
    {
        if (isDragging)
        {
            highlightedCard.transform.position = InputManager.Instance.GetTouchPosition();
            Vector2 dragEndPos = InputManager.Instance.GetTouchPosition();
            dragDistance = Vector2.Distance(dragStartPos, dragEndPos);
            if (dragDistance >= Deadzone)
            {
                if (ghostTower == null)
                {
                    TowerManager.Instance.towerbuilder.ChangeCardMove();
                    highlightedCard.gameObject.SetActive(false);
                    ghostTower = Instantiate(ghostTowerPrefab, InputManager.Instance.GetTouchPosition(), Quaternion.identity);
                    SpriteRenderer ghostsprite = ghostTower.GetComponent<SpriteRenderer>();
                    TowerData towerData = TowerManager.Instance.GetTowerData(highlightedIndex);
                    ghostsprite.sprite = TowerManager.Instance.GetSprite(towerData.TowerIndex);
                }
                else
                {
                    ghostTower.transform.position = InputManager.Instance.GetTouchWorldPosition();
                }
            }
            else if(ghostTower != null)
            {
                TowerManager.Instance.towerbuilder.ChangeCardDontMove();
                highlightedCard.gameObject.SetActive(true);
                Destroy(ghostTower);
            }
        }
    }
    
    /// <summary>
    /// 덱에 카드위치 정렬
    /// </summary>
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
    }

    /// <summary>
    /// 카드 이동 종료시
    /// 1. 드래그 거리가 데드존보다 작으면 카드가 손으로 들어감
    /// 2. 드래그 거리가 데드존보다 크면 카드가 타워설치여부 판별
    /// 3. 타워설치가 가능하면 해당위치에 타워설치 및 카드 사용
    /// 4. 타워설치가 불가능하면 카드가 손으로 복귀
    /// </summary>
    /// <param name="card"></param>

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
                TowerManager.Instance.towerbuilder.ChangeCardDontMove();
                //Destroy(ghostTower);
                //ghostTower = null;
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
                                        TowerManager.Instance.towerbuilder.CardToTowerCombine(InputManager.Instance.GetTouchWorldPosition());
                                        UseCard();
                                    }
                                    else
                                    {
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

    /// <summary>
    /// 카드획득시 이미 덱에 있는 카드라면 스택을 증가시킨다.
    /// 덱에 없는 카드라면 새로운 위치를 부여해주고 해당위치에 카드를 배치한다.
    /// </summary>
    /// <param name="index"></param>
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

    /// <summary>
    /// 카드 클릭시 덱에서 노출시켜주는 메서드
    /// </summary>
    /// <param name="card"></param>
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
        handRect.DOAnchorPos(handRect.anchoredPosition - new Vector2(0, 100f), 0.3f).SetEase(Ease.OutCubic)
            .OnComplete(() => {handRect.anchoredPosition = originalPosition - new Vector2(0, 100f); });
        
    }
    /// <summary>
    /// 중간에 카드 동작을 취소하는 메서드
    /// </summary>
    public void CancleCard()
    {
        if(isDragging) isDragging = false;
        TowerManager.Instance.towerbuilder.ChangeCardDontMove();
        Destroy(ghostTower);
        highlightedCard.gameObject.SetActive(true);
        highlightedCard.transform.position = InputManager.Instance.GetTouchPosition();
        UnHighlightCard();
    }
    /// <summary>
    /// 하이라이트된 카드 동작을 원래위치로 돌려주는 메서드
    /// </summary>
    public void UnHighlightCard()
    {
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
        RectTransform handRect = GetComponent<RectTransform>();
        handRect.DOAnchorPos(originalPosition, 0.3f).SetEase(Ease.OutCubic)
            .OnComplete(() => { handRect.anchoredPosition = originalPosition;});
        highlightedCard = null;
        highlightedIndex = -1;
        highlightedOrder = -1;
        isHighlighting = false;
        TowerManager.Instance.EndInteraction(InteractionState.CardMoving);
    }

    /// <summary>
    /// 카드 사용시 카드가 사라지는 연출
    /// </summary>
    public void UseCard()
    {
        highlightedCard.onClicked -= MoveCardStart;
        highlightedCard.onClickEnd -= MoveCardEnd;
        Destroy(highlightedCard.gameObject);
        ResetHighlightState();
    }

    /// <summary>
    /// 덱에서의 카드의 위치를 계산하는 메서드
    /// </summary>
    /// <param name="index"></param>
    /// <param name="totalCount"></param>
    /// <param name="rect"></param>
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
