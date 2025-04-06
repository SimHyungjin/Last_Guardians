using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Ring,
    Necklace,
    Count
}
public enum EquipmentValueType
{
    Attack,
    Move,
    CriticalChance,
    CriticalDamage,
    AttackRange,
    Penetration,
    ElementalDamage,
    CooldownReduction,
    Count
}

[CreateAssetMenu(menuName = "Data/EquipmentData", fileName = "NewEquipmentData")]
public class EquipmentData : ScriptableObject
{
    [Header("기본 정보")]
    public string equipmentName;
    public EquipmentType equipmentType;
    public Sprite icon;
    public string description;

    [Header("스탯 정보")]
    public StatValue[] stats;
    [System.Serializable]
    public struct StatValue
    {
        public EquipmentValueType type;
        public float value;
    }
}
