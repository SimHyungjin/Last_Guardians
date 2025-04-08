using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HandCardLayout : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    [SerializeField] private Card cardPrefab;
    private float radius = 2f; 
    public float maxAngle = 30f;

    private void Start()
    {
        UpdateLayout();
        foreach (Card card in cards)
        {
            card.onClicked += MoveCardStart;
            card.onClickEnd += MoveCardEnd;
        }
    }
  
    void UpdateLayout()
    {
        int count = cards.Count;
        if (count == 0) return;

        float angleStep = (count > 1) ? (maxAngle * 2) / (count - 1) : 0;

        for (int i = 0; i < count; i++)
        {
            float angle = maxAngle - angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad)) * radius;

            cards[i].transform.localPosition = pos;
            cards[i].transform.localRotation = Quaternion.Euler(0, 0, angle);

            SpriteRenderer sr = cards[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 100+i;
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
                card.AddStack(); 
                card.ShowStack();
                return;
            }
        }
        Card addCard=Instantiate(cardPrefab, transform);
        addCard.Init(index);
        cards.Add(addCard);
        addCard.onClicked += MoveCardStart;
        addCard.transform.SetParent(transform); 
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
