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
public class SelectionController : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer inventorySlotContainer;
    [SerializeField] private ItemPopupController itemPopupController;
    [SerializeField] private UpgradePopupController upgradePopup;

    public ItemInstance selectedData { get; private set; } = null;
    public List<ItemInstance> selectedDataList { get; private set; } = new();

    public SelectionMode selectionMode { get; private set; } = SelectionMode.Single;

    public Action<Slot> SelectSlotListAction;

    public void ToggleMode(SelectionMode selectionMode)
    {
        this.selectionMode = selectionMode;
        selectedData = null;
        selectedDataList.Clear();
        itemPopupController.Close();
        itemPopupController.UpdatePopupUI();
    }

    /// <summary>
    /// 슬롯을 선택합니다. 슬롯을 선택하면 팝업이 열립니다.
    /// </summary>
    /// <param name="slot"></param>
    public void SelectSlot(Slot slot)
    {
        if (selectedData != null && selectedData.UniqueID == slot.GetData().UniqueID) return;

        selectedData = slot.GetData();
        upgradePopup.SetData(selectedData);

        itemPopupController.SetData(selectedData);
        itemPopupController.Open();
        inventorySlotContainer.Refresh();
    }

    public void SelectSlotList(Slot slot)
    {
        ItemInstance data = slot.GetData();
        if (data == null) return;

        for (int i = 0; i < selectedDataList.Count; i++)
        {
            if (selectedDataList[i].UniqueID == data.UniqueID)
            {
                selectedDataList.RemoveAt(i);
                SelectSlotListAction?.Invoke(slot);
                return;
            }
        }
        selectedDataList.Add(data);
        inventorySlotContainer.Refresh();
        SelectSlotListAction?.Invoke(slot);
    }
    /// <summary>
    /// 슬롯을 선택 해제합니다. 슬롯을 선택 해제하면 팝업이 닫힙니다.
    /// </summary>
    public void DeselectSlot()
    {
        selectedData = null;
        itemPopupController.Close();
    }

    /// <summary>
    /// 슬롯을 새로 고칩니다. 슬롯의 데이터를 새로 고칩니다.
    /// </summary>
    /// <param name="instance"></param>
    public void RefreshSlot(ItemInstance instance)
    {
        selectedData = instance;
        itemPopupController.SetData(selectedData);
        upgradePopup.SetData(selectedData);
    }
}