using System;
using System.Collections.Generic;

public class Equipment
{
    private Dictionary<EquipType, EquipmentData> equipped = new();

    public event Action<EquipmentData> OnEquip;
    public event Action<EquipmentData> OnUnequip;

    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }
    public float specialEffectIDs { get; private set; }
    public int specialEffectCount { get; private set; }

    public void Equip(EquipmentData data)
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

    public void UnEquip(EquipmentData data)
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
            specialEffectCount = data.specialEffectID > 0 ? data.specialEffectID : 0;
        }
    }

    public bool IsEquipped(EquipmentData data)
    {
        if (data == null) return false;
        if (equipped.Count == 0) return false;
        if (equipped.TryGetValue(data.equipType, out var d) && d == data) return true;
        else return false;
    }
}
