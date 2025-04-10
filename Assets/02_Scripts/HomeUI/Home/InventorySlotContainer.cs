using System.Collections.Generic;
using UnityEngine;

public class InventorySlotContainer : MonoBehaviour
{
    [SerializeField] private int slotNum = 50;
    private List<Slot> slots = new();

    public void Init()
    {
        for (int i = 0; i < slotNum; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot",this.transform);
            slots.Add(slot);
        }
    }

    public void Display(List<ItemData> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetData(items[i]);

                if (items[i] is EquipmentData eq)
                    slots[i].SetEquipped(HomeManager.Instance.equipment.IsEquipped(eq));
                else
                    slots[i].SetEquipped(false);
            }
            else
            {
                slots[i].Clear();
                slots[i].SetSelected(false);
                slots[i].SetEquipped(false);
                slots[i].SetGradeEffect(false);
            }
        }
    }

    public List<Slot> GetSlots() => slots;
}