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
        equipment = InventoryManager.Instance.equipment;
        equipment.OnEquip += BindSlot;
        equipment.OnUnequip += UnbindSlot;
    }

    public void BindSlot(ItemInstance instance)
    {
        if (instance?.AsEquipData == null) return;
        if (equipSlots.TryGetValue(instance.AsEquipData.equipType, out var slot))
        {
            slot.SetData(instance);
        }
    }
    private void UnbindSlot(ItemInstance instance)
    {
        if (instance?.AsEquipData is not EquipData data) return;

        ClearSlot(data.equipType);
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
