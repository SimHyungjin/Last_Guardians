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

[CreateAssetMenu(menuName = "Data/EquipmentData", fileName = "NewEquipmentData")]
public class EquipemntData : ItemData
{
    public int equipIndex;
    public EquipType equipType;
    public AttackType attackType;

    public float attackPower;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float penetration;
    public float defense;
    public int specialEffectID;
}
