using System.Collections.Generic;

/// <summary>
/// 플레이어의 장비 상태와 장비로부터 얻는 스탯을 관리하는 매니저 클래스입니다.
/// </summary>
public class EquipmentManager
{
    public static EquipmentManager Instance;

    // 현재 장착된 장비를 장비 타입 별로 저장
    public Dictionary<EquipmentType, EquipmentData> equipped = new();
    // 현재 장비들로부터 누적된 스탯 값 저장
    public Dictionary<EquipmentValueType, float> equippedStats = new();

    /// <summary>
    /// 장비를 장착합니다. 같은 타입의 장비가 있다면 먼저 해제하고 새로 장착합니다.
    /// </summary>
    /// <param name="data">장착할 장비 데이터</param>
    public void Equip(EquipmentData data)
    {
        if (data == null) return;
        if (equipped.TryGetValue(data.equipmentType, out var currentEquip))
        {
            UnEquip(currentEquip);
        }
        AddStats(data);
        equipped[data.equipmentType] = data;
    }

    /// <summary>
    /// 장비를 해제하고 해당 스탯을 제거합니다.
    /// </summary>
    /// <param name="data">해제할 장비 데이터</param>
    public void UnEquip(EquipmentData data)
    {
        if (data == null) return;
        SubstractStats(data);
        equipped[data.equipmentType] = null;
    }

    /// <summary>
    /// 장비에서 제공하는 스탯을 더합니다.
    /// </summary>
    /// <param name="data">장착한 장비 데이터</param>
    void AddStats(EquipmentData data)
    {
        if (data == null) return;
        foreach (var stat in data.stats)
        {
            if (equippedStats.ContainsKey(stat.type))
            {
                equippedStats[stat.type] += stat.value;
            }
            else
            {
                equippedStats[stat.type] = stat.value;
            }
        }
    }

    /// <summary>
    /// 장비에서 제공하던 스탯을 감소합니다.
    /// </summary>
    /// <param name="data">해제한 장비 데이터</param>
    void SubstractStats(EquipmentData data)
    {
        if (data == null) return;
        foreach (var stat in data.stats)
        {
            if (equippedStats.ContainsKey(stat.type))
            {
                equippedStats[stat.type] -= stat.value;
            }
        }
    }
    /// <summary>
    /// 스텟을 반환하고 EquipmentValueType이 null이면 0을 반환합니다.
    /// </summary>
    /// <param name="type">스텟 타입</param>
    /// <returns></returns>
    public float GetStat(EquipmentValueType type)
    {
        return equippedStats.TryGetValue(type, out var stat) ? stat : 0;
    }

}
