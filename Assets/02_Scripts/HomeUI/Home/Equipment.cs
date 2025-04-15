using System;
using System.Collections.Generic;

public class Equipment
{
    private Dictionary<EquipType, EquipData> equipped = new();

    public event Action<EquipData> OnEquip;
    public event Action<EquipData> OnUnequip;

    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }
    public float specialEffectIDs { get; private set; }
    public int specialEffectID { get; private set; }

    public void Equip(EquipData data)
    {
        if (data == null) return;
        if (equipped.TryGetValue(data.equipType, out var cur))
        {
            if (cur == data) return;
            UnEquip(cur);
        }
        equipped[data.equipType] = data;
        RecalculateStats();

        OnEquip.Invoke(data);
    }

    public void UnEquip(EquipData data)
    {
        if (data == null || !equipped.ContainsKey(data.equipType)) return;
        equipped.Remove(data.equipType);
        RecalculateStats();

        OnUnequip.Invoke(data);
    }

    void RecalculateStats()
    {
        totalAttack = totalAttackSpeed = totalAttackRange = totalCriticalChance =
            totalCriticalDamage = totalPenetration = totalMoveSpeed = 0;

        foreach (var data in equipped.Values)
        {
            if (data == null) continue;

            totalAttack += data.attackPower;
            totalAttackSpeed += data.attackSpeed;
            totalAttackRange += data.attackRange;
            totalCriticalChance += data.criticalChance;
            totalCriticalDamage += data.criticalDamage;
            totalPenetration += data.penetration;
            totalMoveSpeed += data.moveSpeed;
            specialEffectID = data.specialEffectID > 0 ? data.specialEffectID : 0;
        }
    }

    public bool IsEquipped(EquipData data)
    {
        if (data == null) return false;
        if (equipped.Count == 0) return false;
        if (equipped.TryGetValue(data.equipType, out var d) && d == data && data.uniqueID == d.uniqueID) return true;
        else return false;
    }

    public EquipmentStats ToStats()
    {
        return new EquipmentStats
        {
            attack = totalAttack,
            attackSpeed = totalAttackSpeed,
            attackRange = totalAttackRange,
            criticalChance = totalCriticalChance,
            criticalDamage = totalCriticalDamage,
            penetration = totalPenetration,
            moveSpeed = totalMoveSpeed,
            specialEffectID = specialEffectID
        };
    }
}

public class EquipmentStats
{
    public float attack;
    public float attackSpeed;
    public float attackRange;
    public float criticalChance;
    public float criticalDamage;
    public float penetration;
    public float moveSpeed;
    public int specialEffectID;
}
