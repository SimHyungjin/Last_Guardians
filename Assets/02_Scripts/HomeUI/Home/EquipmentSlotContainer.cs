using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotContainer : MonoBehaviour
{
    [SerializeField] private Transform[] slotParents;

    private Dictionary<EquipType, Slot> slots = new();

    private void Awake()
    {
        for (int i = 0; i < (int)EquipType.Count; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", slotParents[i]);
            slots.Add((EquipType)i, slot);
        }
    }

    public void Refresh()
    {
        foreach (var slot in slots.Values)
        {
            slot.Refresh();
        }
    }

    public void BindEquipment(EquipType type, EquipmentData data)
    {
        if (slots.TryGetValue(type, out var slot))
        {
            slot.SetData(data);
        }
    }

    public void ClearSlot(EquipType type)
    {
        if (slots.TryGetValue(type, out var slot))
        {
            slot.Clear();
        }
    }

    public Dictionary<EquipType, Slot> GetSlots() => slots;
}
