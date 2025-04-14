using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewEquipData", menuName = "Data/Equip Data")]
public class EquipData : ScriptableObject
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

    public void SetData(int index, EquipType eType, AttackType aType, float atkPower, float atkSpeed,
        float mvSpeed, float critChance, float critDamage, float pen, float atkRange, int specialEffectId)
    {
        equipIndex = index;
        equipType = eType;
        attackType = aType;
        attackPower = atkPower;
        attackSpeed = atkSpeed;
        moveSpeed = mvSpeed;
        criticalChance = critChance;
        criticalDamage = critDamage;
        penetration = pen;
        attackRange = atkRange;
        specialEffectID = specialEffectId;
    }
}

