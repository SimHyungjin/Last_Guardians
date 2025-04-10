using System;
using System.Collections.Generic;

public class Equipment
{
    private Dictionary<EquipType, EquipmentData> equipped = new();

    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }
    public float totalDefense { get; private set; }
    public float specialEffectIDs { get; private set; }
    public int specialEffectCount { get; private set; }

    public IReadOnlyDictionary<EquipType, EquipmentData> EquippedItems => equipped;

    public void Equip(EquipmentData data)
    {
        if (data == null) return;
        if (equipped.TryGetValue(data.equipType, out var cur) && cur == data) return;

        equipped[data.equipType] = data;
        RecalculateStats();
    }

    public void UnEquip(EquipmentData data)
    {
        if (data == null || !equipped.ContainsKey(data.equipType)) return;
        equipped.Remove(data.equipType);
        RecalculateStats();
    }

    void RecalculateStats()
    {
        totalAttack = totalAttackSpeed = totalAttackRange = totalCriticalChance =
            totalCriticalDamage = totalPenetration = totalMoveSpeed = totalDefense = 0;

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
            totalDefense += data.defense;
            specialEffectCount = data.specialEffectID > 0 ? 1 : 0;
        }
    }

    public bool IsEquipped(EquipmentData data) =>
        data != null && equipped.TryGetValue(data.equipType, out var d) && d == data;
}
