using UnityEngine;

/// <summary>
/// 인벤토리 슬롯을 선택하는 컨트롤러입니다.
/// </summary>
public class SelectionController : MonoBehaviour
{
    [SerializeField] private ItemPopupController itemPopupController;

    public Slot selectedSlot { get; private set; } = null;
    public ItemInstance selectedData { get; private set; } = null;

    /// <summary>
    /// 슬롯을 선택합니다. 슬롯을 선택하면 팝업이 열립니다.
    /// </summary>
    /// <param name="slot"></param>
    public void SelectSlot(Slot slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedData = slot.GetData();
        selectedSlot.SetSelected(true);
        itemPopupController.Open(slot);
    }

    /// <summary>
    /// 슬롯을 선택 해제합니다. 슬롯을 선택 해제하면 팝업이 닫힙니다.
    /// </summary>
    public void DeselectSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetSelected(false);
            selectedSlot = null;
            selectedData = null;
        }
        itemPopupController.Close();
    }

    /// <summary>
    /// 슬롯을 새로 고칩니다. 슬롯의 데이터를 새로 고칩니다.
    /// </summary>
    /// <param name="instance"></param>
    public void RefreshSlot(ItemInstance instance)
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetData(instance);
            selectedData = instance;
        }
    }
}