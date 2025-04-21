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

    public void Display(IReadOnlyList<ItemInstance> items)
    {
        ApplySlotActions(items, (slot, item) =>
        {
            slot.SetData(item);

            var equipData = item.AsEquipData;
            slot.SetEquipped(equipData != null && equipment.IsEquipped(item));

            if (selectionController.selectedSlot != null)
            {
                slot.SetSelected(selectionController.selectedData != null && slot.GetData()?.UniqueID == selectionController.selectedData.UniqueID);
            }

            slot.Refresh();
        });
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            var instance = slot.GetData();
            if (instance?.AsEquipData != null)
            {
                slot.SetEquipped(equipment.IsEquipped(instance));
            }
            else
            {
                slot.SetEquipped(false);
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

    private void ApplySlotActions(IReadOnlyList<ItemInstance> items, Action<Slot, ItemInstance> action)
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
