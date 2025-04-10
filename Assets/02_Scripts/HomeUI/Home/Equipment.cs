using System.Collections.Generic;

public class Equipment
{
    public EquipmentData curEquippedData;
    public EquipmentSlotContainer equipmentSlotContainer;

    private Dictionary<EquipType, EquipmentData> equipped = new();

    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }
    public float totalDefense { get; private set; }
    public List<int> specialEffectIDs { get; private set; } = new();


    public void Equip(EquipmentData data)
    {
        if (data == null) return;
        equipped[data.equipType] = data;
        RecalculateStats();
    }

    public void UnEquip(EquipmentData data)
    {
        if (data == null) return;
        equipped.Remove(data.equipType);
        RecalculateStats();
    }

    void RecalculateStats()
    {
        totalAttack = 0;
        totalAttackSpeed = 0;
        totalAttackRange = 0;
        totalCriticalChance = 0;
        totalCriticalDamage = 0;
        totalPenetration = 0;
        totalMoveSpeed = 0;
        totalDefense = 0;
        specialEffectIDs.Clear();

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

            if (data.specialEffectID != 0)
                specialEffectIDs.Add(data.specialEffectID);
        }
    }

    public EquipmentData GetEquipped(EquipType type)
    {
        equipped.TryGetValue(type, out var data);
        return data;
    }

    public bool IsEquipped(EquipmentData data)
    {
        if (data == null) return false;

        var equippedData = GetEquipped(data.equipType);
        return equippedData == data;
    }

    public IReadOnlyDictionary<EquipType, EquipmentData> GetAllEquipped() => equipped;
}
