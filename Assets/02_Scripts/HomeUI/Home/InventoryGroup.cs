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

    public void Init()
    {
        inventorySlotContainer.Init();
        equipmentSlotContainer.Init();
        itemPopupController.Init();

        inventorySlotContainer.Display(MainSceneManager.Instance.inventory.GetFilteredView());
        equipmentSlotContainer.BindAll();
        equipmentSlotContainer.Refresh();
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
