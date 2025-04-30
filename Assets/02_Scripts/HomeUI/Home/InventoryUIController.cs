using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer inventorySlotContainer;
    [SerializeField] private InventoryUIButton inventoryUIButton;

    private void Start()
    {
        var inventory = MainSceneManager.Instance.inventory;

        inventoryUIButton.onItemTypeFilter = inventory.SetItemTypeFilter;
        inventoryUIButton.onEquipTypeFilter = inventory.SetEquipTypeFilter;
        inventoryUIButton.onSortButtonClicked = inventory.SetSortType;
    }
}