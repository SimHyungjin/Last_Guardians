using System.Collections.Generic;
using UnityEngine;

public class InventorySlotContainer : MonoBehaviour
{
    [SerializeField] private int slotNum = 10;
    private List<Slot> slots = new();

    public void Init()
    {
        for (int i = 0; i < slotNum; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot",this.transform);
            slots.Add(slot);
        }
    }

    public void UpdateSlots(List<EquipmentData> view)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < view.Count)
                slots[i].SetData(view[i]);
            else
                slots[i].ClearData();
        }
    }

    public List<Slot> GetSlots() => slots;
}