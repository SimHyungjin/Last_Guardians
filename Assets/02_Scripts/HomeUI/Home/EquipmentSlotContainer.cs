using System;
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
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", slotParents[i]);
            equipSlots.Add((EquipType)i, slot);
        }
    }

    private void Start()
    {
        equipment = HomeManager.Instance.equipment;
        equipment.OnEquip += (data) => BindSlot(data);
        equipment.OnUnequip += (data) => ClearSlot(data.equipType);
    }

    public void BindSlot(EquipData data)
    {
        if (data == null) return;
        if (equipSlots.TryGetValue(data.equipType, out var slot))
        {
            slot.SetData(data);
        }
    }
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
            slot.Refresh();
        }
    }

    public IReadOnlyDictionary<EquipType, Slot> GetSlots() => equipSlots;
}
