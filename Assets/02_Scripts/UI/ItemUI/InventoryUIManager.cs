using UnityEngine;

public enum PopupType
{
    Item,
    Upgrade,
    Sell
}
public class InventoryUIManager : MonoBehaviour
{
    public ItemPopupUI itemPopupController;
    public UpgradePopupUI upgradePopupController;
    public SellPopupUI sellPopupController;

    public InventoryUIButtonView inventoryUIButtonView;

    private InventoryManager inventoryManager;

    public void Init()
    {
        itemPopupController.Init();
        upgradePopupController.Init();
        sellPopupController.Init();
        inventoryUIButtonView.Init();
        inventoryManager = MainSceneManager.Instance.inventoryManager;
    }

    private void OnEnable()
    {
        itemPopupController.Close();
        upgradePopupController.Close();
        sellPopupController.Close();
        inventoryUIButtonView.RefreshGoods();
        if(inventoryManager == null ) return;
        inventoryManager.inventorySelectionController.ClearSelect();
        inventoryManager.inventorySelectionController.ClearSelects();
        inventoryManager.inventorySlotContainer.Refresh();
        
    }

    public void OpenPopup(PopupType popupType)
    {
        itemPopupController.Close();
        upgradePopupController.Close();
        sellPopupController.Close();

        switch (popupType)
        {
            case PopupType.Item:
                itemPopupController.Open();
                break;
            case PopupType.Upgrade:
                if (inventoryManager.inventorySlotContainer == null || inventoryManager.inventorySelectionController.selectedData == null) break;
                upgradePopupController.Open();
                break;
            case PopupType.Sell:
                sellPopupController.Open();
                break;
        }
    }
}
