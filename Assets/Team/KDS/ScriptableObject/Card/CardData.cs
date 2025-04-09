using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data" +"/CardData", order = 1)]
public class CardData : ScriptableObject
{
    [SerializeField] private int towerIndex;
    [SerializeField] private Sprite cardImage;

    public int TowerIndex => towerIndex;
    public Sprite CardImage => cardImage;
    public void SetData(int towerIndex, Sprite cardImage)
    {
        this.towerIndex = towerIndex;
        this.cardImage = cardImage;
    }
}
