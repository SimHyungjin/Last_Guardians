using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotContainer : MonoBehaviour
{
    [SerializeField] private Transform[] slotParents;

    private Dictionary<EquipType, Slot> euipmentSlots = new();

    private void Awake()
    {
        for (int i = 0; i < (int)EquipType.Count; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", slotParents[i]);
            euipmentSlots.Add((EquipType)i, slot);
        }
    }

    private void Start()
    {
        HomeManager.Instance.equipment.OnEquip += (data) => BindSlot(data);
        HomeManager.Instance.equipment.OnUnequip += (data) => ClearSlot(data.equipType);
    }

    public void BindSlot(EquipmentData data)
    {
        if (data == null) return;
        if (euipmentSlots.TryGetValue(data.equipType, out var slot))
        {
            slot.SetData(data);
        }
    }
    public void ClearSlot(EquipType type)
    {
        if (euipmentSlots.TryGetValue(type, out var slot))
        {
            slot.Clear();
        }
    }

    public void Refresh()
    {
        foreach (var slot in euipmentSlots.Values)
        {
            slot.Refresh();
        }
    }

    public IReadOnlyDictionary<EquipType, Slot> GetSlots() => euipmentSlots;
}
