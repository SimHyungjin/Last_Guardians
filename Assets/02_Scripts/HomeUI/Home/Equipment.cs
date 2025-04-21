using System;
using System.Collections.Generic;

public class Equipment
{
    private Dictionary<EquipType, ItemInstance> equipped = new();

    public event Action<ItemInstance> OnEquip;
    public event Action<ItemInstance> OnUnequip;

    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }
    public float specialEffectIDs { get; private set; }
    public int specialEffectID { get; private set; }

    public void Equip(ItemInstance instance, bool updateUI = true)
    {
        if (instance?.AsEquipData == null) return;
        EquipData data = instance.AsEquipData;

        if (equipped.TryGetValue(data.equipType, out var cur))
        {
            if (cur.UniqueID == instance.UniqueID) return;
            UnEquip(cur);
        }
        equipped[data.equipType] = instance;

        RecalculateStats();
        if(updateUI) OnEquip.Invoke(instance);
    }

    public void UnEquip(ItemInstance instance, bool updateUI = true)
    {
        if(instance?.AsEquipData == null) return;

        EquipData data = instance.AsEquipData;
        if (!equipped.ContainsKey(data.equipType)) return;

        equipped.Remove(data.equipType);

        RecalculateStats();
        if(updateUI) OnUnequip.Invoke(instance);
    }

    void RecalculateStats()
    {
        totalAttack = totalAttackSpeed = totalAttackRange = totalCriticalChance =
            totalCriticalDamage = totalPenetration = totalMoveSpeed = 0;

        foreach (var instance in equipped.Values)
        {
            if (instance?.AsEquipData is not EquipData data) continue;

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

    public bool IsEquipped(ItemInstance instance)
    {
        if (instance?.AsEquipData == null) return false;
        if (equipped.Count == 0) return false;
        return equipped.TryGetValue(instance.AsEquipData.equipType, out var cur) && cur.UniqueID == instance.UniqueID;
    }

    public IReadOnlyDictionary<EquipType, ItemInstance> GetEquipped() => equipped;

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
