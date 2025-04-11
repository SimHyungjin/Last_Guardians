using System.Collections.Generic;
using System;
using UnityEngine;

public class InventorySlotContainer : MonoBehaviour
{
    [SerializeField] private int slotNum = 50;
    private List<Slot> slots = new();

    private void Awake()
    {
        for (int i = 0; i < slotNum; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", transform);
            slots.Add(slot);
        }
    }

    private void Start()
    {
        HomeManager.Instance.inventory.OnInventoryChanged += () => Display(HomeManager.Instance.inventory.GetFilteredView());
    }

    public void Display(IReadOnlyList<ItemData> items)
    {
        ApplySlotActions(items, (slot, item) =>
        {
            slot.SetData(item);

            if (item is EquipmentData eq)
            {
                slot.SetEquipped(HomeManager.Instance.equipment.IsEquipped(eq));
            }
            else
            {
                slot.SetEquipped(false);
            }

            if (HomeManager.Instance.selectionController.selectedSlot != null)
            {
                slot.SetSelected(slot.GetData() == HomeManager.Instance.selectionController.selectedData);
            }

            slot.Refresh();
        });
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            if (slot.GetData() is EquipmentData data)
            {
                bool isEquipped = HomeManager.Instance.equipment.IsEquipped(data);
                slot.SetEquipped(isEquipped);
            }
            slot.Refresh();
        }
    }

    public void Clear()
    {
        foreach (var slot in slots)
        {
            slot.Clear();
        }
    }

    private void ApplySlotActions(IReadOnlyList<ItemData> items, Action<Slot, ItemData> action)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                action.Invoke(slots[i], items[i]);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public IReadOnlyList<Slot> GetSlots() => slots;
}
