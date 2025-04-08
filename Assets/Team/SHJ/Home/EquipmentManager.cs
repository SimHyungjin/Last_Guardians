using System.Collections.Generic;

/// <summary>
/// 플레이어의 장비 상태와 장비로부터 얻는 스탯을 관리하는 매니저 클래스입니다.
/// </summary>
public class EquipmentManager
{
    public static EquipmentManager Instance;

    public EquipmentData curEquipeedData;

    // 현재 장착된 장비를 장비 타입 별로 저장
    public Dictionary<ItemType, EquipmentData> equipped = new();

    public float totalAttack { get;private set; }
    public float totalAttackSpeed { get; private set; }
    public float totalAttackRange { get; private set; }
    public float totalCriticalChance { get; private set; }
    public float totalCriticalDamage { get; private set; }
    public float totalPenetration { get; private set; }
    public float totalMoveSpeed { get; private set; }



    /// <summary>
    /// 장비를 장착합니다. 같은 타입의 장비가 있다면 먼저 해제하고 새로 장착합니다.
    /// </summary>
    /// <param name="data">장착할 장비 데이터</param>
    public void Equip(EquipmentData data)
    {
        if (data == null) return;
        if (equipped.TryGetValue(data.itemType, out var currentEquip))
        {
            UnEquip(currentEquip);
        }
        AddStats(data);
        equipped[data.itemType] = data;
    }

    /// <summary>
    /// 장비를 해제하고 해당 스탯을 제거합니다.
    /// </summary>
    /// <param name="data">해제할 장비 데이터</param>
    public void UnEquip(EquipmentData data)
    {
        if (data == null) return;
        SubstractStats(data);
        equipped[data.itemType] = null;
    }

    /// <summary>
    /// 장비에서 제공하는 스탯을 더합니다.
    /// </summary>
    /// <param name="data">장착한 장비 데이터</param>
    void AddStats(EquipmentData data)
    {
        if (data == null) return;

        totalAttack += data.item_attack;
        totalAttackSpeed += data.item_attackSpeed;
        totalAttackRange += data.item_attackRange;
        totalCriticalChance += data.item_criticalChance;
        totalCriticalDamage += data.item_criticalDamage;
        totalPenetration += data.item_pentration;
        totalMoveSpeed += data.item_moveSpeed;
    }

    /// <summary>
    /// 장비에서 제공하던 스탯을 감소합니다.
    /// </summary>
    /// <param name="data">해제한 장비 데이터</param>
    void SubstractStats(EquipmentData data)
    {
        if (data == null) return;

        totalAttack -= data.item_attack;
        totalAttackSpeed -= data.item_attackSpeed;
        totalAttackRange -= data.item_attackRange;
        totalCriticalChance -= data.item_criticalChance;
        totalCriticalDamage -= data.item_criticalDamage;
        totalPenetration -= data.item_pentration;
        totalMoveSpeed -= data.item_moveSpeed;

    }



}
