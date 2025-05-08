using UnityEngine;

/// <summary>
/// 인벤토리관련 오브젝트를 연결하는 클래스입니다.
/// </summary>
public class InventoryGroup : MonoBehaviour
{
    public InventorySlotContainer inventorySlotContainer;
    public EquipmentSlotContainer equipmentSlotContainer;
    public SelectionController selectionController;
    public ItemPopupController itemPopupController;
    public InventoryUI inventoryUI;

    public void Init()
    {
        inventorySlotContainer.Init();
        equipmentSlotContainer.Init();
        itemPopupController.Init();
        inventoryUI.Init();

        inventorySlotContainer.Display(MainSceneManager.Instance.inventory.GetFilteredView());
        equipmentSlotContainer.BindAll();
        equipmentSlotContainer.Refresh();

        if(MainSceneManager.Instance.inventory.GetAll().Count == 0) itemPopupController.Close();
        else itemPopupController.Open();
    }
    //private void OnEnable()
    //{
    //    StartCoroutine(DelayedClose());
    //}

    //private IEnumerator DelayedClose()
    //{
    //    yield return null;
    //    itemPopupController.Close();
    //}
}
