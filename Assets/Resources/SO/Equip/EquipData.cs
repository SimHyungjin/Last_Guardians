using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewEquip", menuName = "Data/Equip Data", order = 1)]
public class EquipData : ItemData
{
    [Header("장비 능력치")]
    [SerializeField] private int equipIndex;
    [SerializeField] private EquipType equipType;
    [SerializeField] private AttackType attackType;
    [SerializeField] private float attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float penetration;
    [SerializeField] private float attackRange;
    [SerializeField] private int specialEffectID;
    [SerializeField] private ItemData linkedItem;

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
                linkedItem.ItemSellPrice
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
            item.ItemSellPrice
        );

        icon = item.Icon; // 아이콘도 복사

        linkedItem = item; // 링크된 아이템 설정
    }






}



