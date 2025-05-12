using UnityEngine;

/// <summary>
/// 인벤토리관련 오브젝트를 연결하는 클래스입니다.
/// </summary>
public class InventoryGroup : MonoBehaviour
{
    public EquipmentSlotContainer equipmentSlotContainer;
    public InventorySlotContainer inventorySlotContainer;
    public ItemConnecter itemConnecter;

    public InventoryUI inventoryUI;

    public void Init()
    {
        inventorySlotContainer.Init();
        equipmentSlotContainer.Init();
        itemConnecter.Init();
        inventoryUI.Init();

        inventorySlotContainer.Display(MainSceneManager.Instance.inventory.GetFilteredView());
        equipmentSlotContainer.BindAll();
        equipmentSlotContainer.Refresh();
    }
    
}
