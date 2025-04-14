using System.Collections.Generic;
using System;
using UnityEngine;

public class InventorySlotContainer : MonoBehaviour
{
    [SerializeField] private int slotNum = 50;
    private List<Slot> slots = new();

    private Inventory inventory;
    private Equipment equipment;
    private SelectionController selectionController;

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
        var home = HomeManager.Instance;
        inventory = home.inventory;
        equipment = home.equipment;
        selectionController = home.selectionController;

        inventory.OnInventoryChanged += () => Display(inventory.GetFilteredView());
    }

    public void Display(IReadOnlyList<ItemData> items)
    {
        ApplySlotActions(items, (slot, item) =>
        {
            slot.SetData(item);

            if (item is EquipData eq)
            {
                slot.SetEquipped(equipment.IsEquipped(eq));
            }
            else
            {
                slot.SetEquipped(false);
            }

            if (selectionController.selectedSlot != null)
            {
                slot.SetSelected(slot.GetData() == selectionController.selectedData);
            }

            slot.Refresh();
        });
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            if (slot.GetData() is EquipData data)
            {
                bool isEquipped = equipment.IsEquipped(data);
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
