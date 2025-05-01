using System;
using System.Collections.Generic;

/// <summary>
/// 장비를 관리하는 클래스입니다.
/// </summary>
public class Equipment
{
    private Dictionary<EquipType, ItemInstance> equipped = new();

    public event Action<ItemInstance> OnEquip;
    public event Action<ItemInstance> OnUnequip;

    public AttackType changeAttackType { get; private set; }
    public float totalAttack { get; private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }
    public int specialEffectID { get; private set; }

    /// <summary>
    /// 장비를 장착합니다. 이미 있을 경우 해제 후 장착합니다.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="updateUI"></param>
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
        if (updateUI) OnEquip?.Invoke(instance);
    }

    /// <summary>
    /// 장비를 해제합니다.장비가 없을 경우 아무것도 하지 않습니다.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="updateUI"></param>
    public void UnEquip(ItemInstance instance, bool updateUI = true)
    {
        if (instance?.AsEquipData == null) return;

        EquipData data = instance.AsEquipData;
        if (!equipped.ContainsKey(data.equipType)) return;

        equipped.Remove(data.equipType);

        RecalculateStats();
        if (updateUI) OnUnequip?.Invoke(instance);
    }
    /// <summary>
    /// 장비의 능력치를 계산하여 업데이트합니다.
    /// </summary>
    void RecalculateStats()
    {
        changeAttackType = AttackType.Melee;
        totalAttack = totalAttackSpeed = totalAttackRange = totalCriticalChance =
            totalCriticalDamage = totalPenetration = totalMoveSpeed = 0;

        foreach (var instance in equipped.Values)
        {
            if (instance?.AsEquipData is not EquipData data) continue;

            if (data.equipType == EquipType.Weapon) changeAttackType = data.attackType;

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
    /// <summary>
    /// 장비가 장착되어 있는지 확인합니다.
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public bool IsEquipped(ItemInstance instance)
    {
        if (instance?.AsEquipData == null) return false;
        if (equipped.Count == 0) return false;
        return equipped.TryGetValue(instance.AsEquipData.equipType, out var cur) && cur.UniqueID == instance.UniqueID;
    }

    public IReadOnlyDictionary<EquipType, ItemInstance> GetEquipped() => equipped;

    /// <summary>
    /// 장비의 능력치를 플레이어에게 전달하기 위한 정보를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public EquipmentInfo InfoToPlayer()
    {
        return new EquipmentInfo
        {
            attackType = changeAttackType,
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

public class EquipmentInfo
{
    public AttackType attackType;
    public float attack;
    public float attackSpeed;
    public float attackRange;
    public float criticalChance;
    public float criticalDamage;
    public float penetration;
    public float moveSpeed;
    public int specialEffectID;
}
