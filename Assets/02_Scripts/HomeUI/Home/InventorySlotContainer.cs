using System.Collections.Generic;
using UnityEngine;

public class InventorySlotContainer : MonoBehaviour
{
    [SerializeField] private int slotNum = 50;
    
    private List<Slot> slots = new();

    public void Awake()
    {
        for (int i = 0; i < slotNum; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", transform);
            slots.Add(slot);
        }
    }

    public void Display(IReadOnlyList<ItemData> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetData(items[i]);

                if (items[i] is EquipmentData eq)
                {
                    slots[i].SetEquipped(HomeManager.Instance.equipment.IsEquipped(eq));
                }
                else
                {
                    slots[i].SetEquipped(false);
                }
                if(HomeManager.Instance.selectionController.selectedSlot != null)
                    slots[i].SetSelected(slots[i].GetData() == HomeManager.Instance.selectionController.selectedData);
                slots[i].SetGradeEffect();
            }
            else
            {
                slots[i].Clear();
            }
        }
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
}
