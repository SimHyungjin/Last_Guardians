using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private ItemPopupController itemPopupController;
    private Slot selectedSlot;

    public void SelectSlot(Slot slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);
        itemPopupController.Open(slot);
    }

    public void DeselectSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetSelected(false);
            selectedSlot = null;
        }
        itemPopupController.Close();
    }
}
