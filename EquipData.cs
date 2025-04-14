using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    Weapon,
    Armor,
    Accessory
}

public enum AttackType
{
    Melee,
    Ranged,
    Magic
}

[CreateAssetMenu(fileName = "NewEquip", menuName = "Data/Equip Data", order = 2)]
public class EquipData : ItemData
{
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

    public int EquipIndex => equipIndex;
    public EquipType EquipType => equipType;
    public AttackType AttackType => attackType;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float MoveSpeed => moveSpeed;
    public float CriticalChance => criticalChance;
    public float CriticalDamage => criticalDamage;
    public float Penetration => penetration;
    public float AttackRange => attackRange;
    public int SpecialEffectID => specialEffectID;

    public void SetEquipData(
        int equipIndex, EquipType equipType, AttackType attackType,
        float attackPower, float attackSpeed, float moveSpeed,
        float criticalChance, float criticalDamage, float penetration,
        float attackRange, int specialEffectID)
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
}


