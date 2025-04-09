using System.Collections.Generic;

public class Equipment
{
    private static Equipment _instance;
    public static Equipment Instance => _instance ??= new Equipment();

    public EquipmentData curEquippedData;

    private Dictionary<ItemType, EquipmentData> equipped = new();

    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }

    public void Equip(EquipmentData data)
    {
        if (data == null) return;

        if (equipped.TryGetValue(data.itemType, out var currentEquip))
        {
            UnEquip(currentEquip);
        }

        equipped[data.itemType] = data;
        RecalculateStats();
    }

    public void UnEquip(EquipmentData data)
    {
        if (data == null) return;

        equipped.Remove(data.itemType);
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

        foreach (var kv in equipped.Values)
        {
            if (kv == null) continue;
            totalAttack += kv.item_attack;
            totalAttackSpeed += kv.item_attackSpeed;
            totalAttackRange += kv.item_attackRange;
            totalCriticalChance += kv.item_criticalChance;
            totalCriticalDamage += kv.item_criticalDamage;
            totalPenetration += kv.item_penetration;
            totalMoveSpeed += kv.item_moveSpeed;
        }
    }
}
