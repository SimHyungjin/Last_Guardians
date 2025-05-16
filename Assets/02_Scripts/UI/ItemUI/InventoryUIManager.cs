using UnityEngine;

public enum PopupType
{
    Item,
    Upgrade,
    Sell
}
public class InventoryUIManager : MonoBehaviour
{
    public ItemSelectionController selectionController;

    public ItemPopupUI itemPopupController;
    public UpgradePopupUI upgradePopupController;
    public SellPopupUI sellPopupController;

    public void Init()
    {
        itemPopupController.Init();
        upgradePopupController.Init();
        sellPopupController.Init();

        itemPopupController.Close();
        upgradePopupController.Close();
        sellPopupController.Close();
    }

    //private void OnEnable()
    //{
    //    itemPopupController.Close();
    //    upgradePopupController.Close();
    //    sellPopupController.Close();
    //}

    public void OpenPopup(PopupType popupType)
    {
        if ((popupType == PopupType.Item && itemPopupController.IsOpen) ||
            (popupType == PopupType.Upgrade && upgradePopupController.IsOpen) ||
            (popupType == PopupType.Sell && sellPopupController.IsOpen))
            return;

        itemPopupController.Close();
        upgradePopupController.Close();
        sellPopupController.Close();

        switch (popupType)
        {
            case PopupType.Item:
                itemPopupController.Open();
                break;
            case PopupType.Upgrade:
                upgradePopupController.Open();
                break;
            case PopupType.Sell:
                sellPopupController.Open();
                break;
        }
    }
}
