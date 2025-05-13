using UnityEngine;

public enum PopupType
{
    Item,
    Upgrade,
    Sell
}
public class ItemConnecter : MonoBehaviour
{
    public SelectionController selectionController;

    public ItemPopupController itemPopupController;
    public UpgradePopupController upgradePopupController;
    public SellPopupContoroller sellPopupController;

    private void OnEnable()
    {
        itemPopupController.Close();
        upgradePopupController.Close();
        sellPopupController.Close();

        selectionController.RefreshSlot(null);
    }

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

        //itemPopupController.UpdatePopupUI();
    }
}
