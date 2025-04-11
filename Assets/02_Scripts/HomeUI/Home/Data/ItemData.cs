using UnityEngine;

public enum ItemType
{
    Equipment,
    UpgradeStone,
}

public enum ItemGrade
{
    Normal,
    Rare,
    Unique,
    Hero,
    Legend
}
[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Tower Data", order = 1)]
public abstract class ItemData : ScriptableObject
{
    public int itemIndex;
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public ItemGrade itemGrade;
    public int itemStackLimit;
    public float itemDropRate;
    public int itemSellPrice;
    public Sprite icon;
}