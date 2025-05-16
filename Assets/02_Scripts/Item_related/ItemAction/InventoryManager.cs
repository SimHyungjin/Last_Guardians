using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlotContainer inventorySlotContainer;
    public EquipmentSlotContainer equipmentSlotContainer;

    public ItemActionHandler itemActionHandler;
    public ItemSelectionController inventorySelectionController;

    public InventoryUIManager inventoryUIManager;

    public void Init()
    {
        inventorySlotContainer.Init();
        equipmentSlotContainer.Init();
    }
}
