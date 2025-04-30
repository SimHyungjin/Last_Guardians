using System.Collections.Generic;
using System;
using UnityEngine;

public class InventorySlotContainer : MonoBehaviour
{
    private int slotNum = 18;
    private List<Slot> slots = new();
    [SerializeField] private RectTransform slotContainerView;

    private Inventory inventory;
    private Equipment equipment;
    private SelectionController selectionController;

    private void Awake()
    {
        for (int i = 0; i < slotNum; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", transform);
            slot.gameObject.SetActive(false);
            slots.Add(slot);
        }
    }
    public void Init()
    {
        var home = MainSceneManager.Instance;
        inventory = home.inventory;
        equipment = home.equipment;
        selectionController = home.inventoryGroup.selectionController; 

        inventory.OnInventoryChanged += () => Display(inventory.GetFilteredView()); 
    }
    private Slot GetOrCreateSlot()
    {
        foreach (var slot in slots)
        {
            if (!slot.gameObject.activeSelf)
                return slot;
        }
        var newSlot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", slotContainerView);
        newSlot.gameObject.SetActive(false);
        slots.Add(newSlot);
        return newSlot;
    }
    private void RectSizeValue(int activeSlotCount)
    {
        int rowCount = Mathf.CeilToInt((float)activeSlotCount / 6f);
        float newHeight = rowCount * 180f;

        var size = slotContainerView.sizeDelta;
        size.y = newHeight;
        slotContainerView.sizeDelta = size;
    }

    public void Display(IReadOnlyList<ItemInstance> items)
    {
        foreach (var slot in slots)
            slot.gameObject.SetActive(false);

        bool hasSelection = selectionController.selectedSlot != null;
        int selectedID = selectionController.selectedData?.UniqueID ?? -1;

        for (int i = 0; i < items.Count; i++)
        {
            var slot = GetOrCreateSlot();
            slot.gameObject.SetActive(true);

            slot.SetData(items[i]);

            var equipData = items[i].AsEquipData;
            slot.SetEquipped(equipData != null && equipment.IsEquipped(items[i]));
            slot.SetSelected(hasSelection && items[i].UniqueID == selectedID);
            slot.Refresh();
        }

        RectSizeValue(items.Count);
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
    public IReadOnlyList<Slot> GetSlots() => slots;
}
