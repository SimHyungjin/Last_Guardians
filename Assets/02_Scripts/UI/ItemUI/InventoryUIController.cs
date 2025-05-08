using UnityEngine;

/// <summary>
/// 필터와 관련된 버튼을 연결하기 위한 UI 컨트롤러입니다.
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer inventorySlotContainer;
    [SerializeField] private InventoryUI inventoryUIButton;

    private void Start()
    {
        var inventory = MainSceneManager.Instance.inventory;

        inventoryUIButton.onItemTypeFilter = inventory.SetItemTypeFilter;
        inventoryUIButton.onEquipTypeFilter = inventory.SetEquipTypeFilter;
        inventoryUIButton.onSortButtonClicked = inventory.SetSortType;
    }
}