using UnityEngine;

public enum ItemType
{
    Weapon,
    Helmet,
    Armor,
    Shoes,
    Ring,
    Neklace,
    Count
}

public enum Item_attackType
{
    Melee,
    Ranged,
    Area
}

public enum Item_Grade
{
    Normal,
    Rare,
    Unique,
    Hero,
    Legend
}


[CreateAssetMenu(menuName = "Data/EquipmentData", fileName = "NewEquipmentData")]
public class EquipmentData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public ItemType itemType;
    public Item_attackType attackType;
    public Item_Grade grade;
    public Sprite icon;

    [Header("스탯 정보")]
    public float item_attack;
    public float item_attackSpeed;
    public float item_attackRange;
    public float item_criticalChance;
    public float item_criticalDamage;
    public float item_pentration;
    public float item_moveSpeed;
}
