using DG.Tweening;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HandCardLayout : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    [SerializeField] private Card cardPrefab;
    private float radius = 500f;
    public float maxAngle = 30f;

    void UpdateLayout()
    {
        int count = cards.Count;
        if (count == 0) return;

        float dynamicMaxAngle = Mathf.Min(9f * (count - 1), 36f);
        float angleStep = (count > 1) ? (dynamicMaxAngle * 2) / (count - 1) : 0;

        for (int i = 0; i < count; i++)
        {
            float angle = dynamicMaxAngle - angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * radius;

            RectTransform rect = cards[i].GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = pos;
                rect.localEulerAngles = new Vector3(0, 0, angle);
                rect.SetSiblingIndex(i);
            }
        }
    }


    public void MoveCardStart(Card card)
    {
        Debug.Log("Card Clicked: " + card.TowerIndex);
    }

    public void MoveCardEnd(Card card)
    {
        Debug.Log("Card Clicked End: " + card.TowerIndex);
    }

    public void AddCard(int index)
    {
        foreach (Card card in cards)
        {
            if (card.TowerIndex == index)
            {
                Debug.Log("Card already exists: " + card.TowerIndex);   
                card.AddStack();
                card.ShowStack();
                return;
            }
        }
        Debug.Log("Card added: " + index);
        Card addCard = Instantiate(cardPrefab, transform);
        addCard.Init(index);
        addCard.onClicked += MoveCardStart;
        addCard.onClickEnd += MoveCardEnd;
        cards.Add(addCard);
        UpdateLayout(); 
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
                cards.Remove(card);
                Destroy(card.gameObject);
                card.onClickEnd += MoveCardEnd;
                UpdateLayout();
                return;
            }
        }
    }
}
