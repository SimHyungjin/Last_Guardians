using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    Weapon,
    Helmet,
    Armor,
    Shoes,
    Ring,
    Necklace,
    Count
}

public enum AttackType
{
    Melee,
    Ranged,
    Area
}


[CreateAssetMenu(fileName = "NewEquip", menuName = "Data/Equip Data", order = 1)]
public class EquipData : ItemData
{
    [field : Header("장비 능력치")]
    [field : SerializeField] public int equipIndex { get; private set; }
    [field : SerializeField] public EquipType equipType { get; private set; }
    [field : SerializeField] public AttackType attackType { get; private set; }
    [field : SerializeField] public float attackPower { get; private set; }
    [field : SerializeField] public float attackSpeed { get; private set; }
    [field : SerializeField] public float moveSpeed { get; private set; }
    [field : SerializeField] public float criticalChance { get; private set; }
    [field : SerializeField] public float criticalDamage { get; private set; }
    [field : SerializeField] public float penetration { get; private set; }
    [field : SerializeField] public float attackRange { get; private set; }
    [field : SerializeField] public int specialEffectID { get; private set; }
    [field : SerializeField] public ItemData linkedItem { get; private set; }

    public ItemData LinkedItem => linkedItem;



    

    public void SetEquipData(
    int equipIndex,
    EquipType equipType,
    AttackType attackType,
    float attackPower,
    float attackSpeed,
    float moveSpeed,
    float criticalChance,
    float criticalDamage,
    float penetration,
    float attackRange,
    int specialEffectID
)
    {
        this.equipIndex = equipIndex;
        this.equipType = equipType;
        this.attackType = attackType;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.penetration = penetration;
        this.attackRange = attackRange;
        this.specialEffectID = specialEffectID;
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (linkedItem != null)
        {
            
            SetData(
                linkedItem.ItemIndex,
                linkedItem.ItemName,
                linkedItem.ItemDescript,
                linkedItem.ItemType,
                linkedItem.ItemGrade,
                linkedItem.ItemStackLimit,
                linkedItem.ItemDropRate,
                linkedItem.ItemSellPrice, 
                linkedItem.ItemApartPrice
            );
        }
#endif
    }
    public void ApplyBaseItemData(ItemData item)
    {
        SetData(
            item.ItemIndex,
            item.ItemName,
            item.ItemDescript,
            item.ItemType,
            item.ItemGrade,
            item.ItemStackLimit,
            item.ItemDropRate,
            item.ItemSellPrice,
            item.ItemApartPrice
        );

        icon = item.Icon; // 아이콘도 복사

        linkedItem = item; // 링크된 아이템 설정
    }






}



