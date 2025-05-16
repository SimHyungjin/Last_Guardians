using System;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionMode
{
    Single,
    Multi
}
/// <summary>
/// 인벤토리 슬롯을 선택하는 컨트롤러입니다.
/// </summary>
public class ItemSelectionController : MonoBehaviour
{
    public SelectionMode selectionMode { get; private set; } = SelectionMode.Single;

    public ItemInstance selectedData { get; private set; } = null;
    public List<ItemInstance> selectedDataList { get; private set; } = new();

    public Action<ItemInstance> selectSlot;
    public Action selectSlotListAction;

    public void ToggleMode(SelectionMode selectionMode)
    {
        this.selectionMode = selectionMode;
        selectedData = null;
        selectedDataList.Clear();
    }
    public void SetSelected(Slot slot)
    {
        selectedData = slot.GetData();
        selectSlot?.Invoke(slot.GetData());
    }
    public void SetSelected(ItemInstance instance)
    {
        selectedData = instance;
        selectSlot?.Invoke(instance);
    }

    public void SelectSlotList(Slot slot)
    {
        if (slot == null) return;

        ItemInstance data = slot.GetData();
        if (data == null) return;

        int index = selectedDataList.FindIndex(i => i.UniqueID == data.UniqueID);
        if (index >= 0)
        {
            selectedDataList.RemoveAt(index);
        }
        else
        {
            if (selectedDataList.Count >= 20) return;
            selectedDataList.Add(data);
        }

        selectSlotListAction?.Invoke();
    }

    public void ClearSelect()
    {
        selectedData = null;
    }
    public void ClearSelects()
    {
        selectedDataList.Clear();
    }
}