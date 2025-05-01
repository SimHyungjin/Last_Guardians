using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 슬롯을 관리하는 클래스입니다.
/// </summary>
public class InventorySlotContainer : MonoBehaviour
{
    private int slotBlockSize = 8;
    private int baseSlotCount = 32;
    private float slotHeight = 120;
    private List<Slot> slots = new();
    [SerializeField] private RectTransform slotContainerView;

    private Inventory inventory;
    private Equipment equipment;
    private SelectionController selectionController;

    private void Awake()
    {
        CreateSlotCount(baseSlotCount);
    }
    public void Init()
    {
        var home = MainSceneManager.Instance;
        inventory = home.inventory;
        equipment = home.equipment;
        selectionController = home.inventoryGroup.selectionController;

        inventory.OnInventoryChanged += () => Display(inventory.GetFilteredView());
    }
    /// <summary>
    /// 슬롯을 생성합니다. 슬롯의 개수를 늘립니다.
    /// </summary>
    /// <param name="count"></param>
    private void CreateSlotCount(int count)
    {
        int existingCount = slots.Count;
        for (int i = existingCount; i < count; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", transform);
            slot.gameObject.SetActive(false);
            slots.Add(slot);
        }
    }
    /// <summary>
    /// 슬롯 뷰 크기를 조정합니다. 슬롯의 개수에 따라 높이를 조정합니다.
    /// </summary>
    /// <param name="activeSlotCount"></param>
    private void RectSizeValue(int activeSlotCount)
    {
        int rowCount = Mathf.CeilToInt((float)activeSlotCount / slotBlockSize);
        float newHeight = rowCount * slotHeight;

        var size = slotContainerView.sizeDelta;
        size.y = newHeight;
        slotContainerView.sizeDelta = size;
    }
    /// <summary>
    /// 슬롯을 표시합니다. 슬롯에 아이템을 바인딩합니다.
    /// </summary>
    /// <param name="items"></param>
    public void Display(IReadOnlyList<ItemInstance> items)
    {
        int targetCount = Mathf.Max(baseSlotCount, Mathf.CeilToInt(items.Count / (float)slotBlockSize) * slotBlockSize);
        CreateSlotCount(targetCount);

        for (int i = 0; i < slots.Count; i++)
            slots[i].gameObject.SetActive(i < targetCount);

        bool hasSelection = selectionController?.selectedSlot != null;
        int selectedID = selectionController?.selectedData?.UniqueID ?? -1;

        for (int i = 0; i < targetCount; i++)
        {
            var slot = slots[i];

            if (i < items.Count)
            {
                var item = items[i];
                slot.SetData(item);
                slot.SetEquipped(item.AsEquipData != null && equipment.IsEquipped(item));
                slot.SetSelected(hasSelection && item.UniqueID == selectedID);
                slot.Refresh();
            }
            else
            {
                slot.Clear();
            }
        }

        RectSizeValue(targetCount);
    }

    /// <summary>
    /// 슬롯을 새로고침합니다. 슬롯의 장비 상태를 업데이트합니다.
    /// </summary>
    public void Refresh()
    {
        foreach (var slot in slots)
        {
            var instance = slot.GetData();
            if (instance?.AsEquipData != null) slot.SetEquipped(equipment.IsEquipped(instance));
            else slot.SetEquipped(false);
            slot.Refresh();
        }
    }

    public void Clear()
    {
        foreach (var slot in slots) slot.Clear();
    }
    public IReadOnlyList<Slot> GetSlots() => slots;
}
