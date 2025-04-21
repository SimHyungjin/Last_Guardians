using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer inventorySlotContainer;
    [SerializeField] private InventoryUIButton inventoryUIButton;

    private void Start()
    {
        var inventory = HomeManager.Instance.inventory;

        inventoryUIButton.onItemTypeFilter.AddListener((itemType) => {inventory.SetItemType(itemType);});
        inventoryUIButton.onEquipTypeFilter.AddListener((equipType) => {inventory.SetEquipTypeFilter(equipType);});
        inventoryUIButton.onSortButtonClicked.AddListener(inventory.SetSortType);
    }
}