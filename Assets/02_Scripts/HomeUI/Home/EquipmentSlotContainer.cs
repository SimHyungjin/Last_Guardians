using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotContainer : MonoBehaviour
{
    [SerializeField] private Transform[] slotParents;

    private Dictionary<EquipType, Slot> equipSlots = new();

    private Equipment equipment;

    private void Awake()
    {
        for (int i = 0; i < (int)EquipType.Count; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", slotParents[i]);
            equipSlots.Add((EquipType)i, slot);
        }
    }

    public void Init()
    {
        equipment = MainSceneManager.Instance.equipment;
        equipment.OnEquip += BindSlot;
        equipment.OnUnequip += UnbindSlot;
    }

    public void BindAll()
    {
        foreach (var kvp in equipment.GetEquipped())
        {
            BindSlot(kvp.Value);
        }
    }

    /// <summary>
    /// 장비 슬롯에 장비를 바인딩합니다.
    /// </summary>
    /// <param name="instance"></param>
    public void BindSlot(ItemInstance instance)
    {
        if (instance?.AsEquipData == null) return;
        if (equipSlots.TryGetValue(instance.AsEquipData.equipType, out var slot))
        {
            slot.SetData(instance);
        }
    }
    /// <summary>
    /// 장비 슬롯에 장비를 언바인딩합니다.
    /// </summary>
    /// <param name="instance"></param>
    private void UnbindSlot(ItemInstance instance)
    {
        if (instance?.AsEquipData is not EquipData data) return;

        ClearSlot(data.equipType);
    }
    /// <summary>
    /// 장비 슬롯을 초기화합니다. 장비가 해제됩니다.
    /// </summary>
    /// <param name="type"></param>
    public void ClearSlot(EquipType type)
    {
        if (equipSlots.TryGetValue(type, out var slot))
        {
            slot.Clear();
        }
    }

    public void Refresh()
    {
        foreach (var slot in equipSlots.Values)
        {
            var instance = slot.GetData();
            //if (instance?.AsEquipData != null) slot.SetEquipped(equipment.IsEquipped(instance));
            //else slot.SetEquipped(false);
            slot.Refresh();
        }
    }

    public IReadOnlyDictionary<EquipType, Slot> GetSlots() => equipSlots;
}
