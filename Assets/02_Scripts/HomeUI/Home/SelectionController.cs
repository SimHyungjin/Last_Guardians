using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private ItemPopupController itemPopupController;

    public Slot selectedSlot { get; private set; } = null;
    public ItemData selectedData { get; private set; } = null;

    public void SelectSlot(Slot slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedData = slot.GetData();
        selectedSlot.SetSelected(true);
        itemPopupController.Open(slot);
    }

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
}