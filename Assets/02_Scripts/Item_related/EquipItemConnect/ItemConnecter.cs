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
    public SellPopupContoroller sellPopupContoroller;

    public void Init()
    {
        itemPopupController.Init();
    }

    private void OnEnable()
    {
        itemPopupController.gameObject.SetActive(false);
        sellPopupContoroller.gameObject.SetActive(false);
        upgradePopupController.gameObject.SetActive(false);
    }

    public void OpenPopup(PopupType popupType)
    {
        upgradePopupController.gameObject.SetActive(false);
        sellPopupContoroller.gameObject.SetActive(false);

        switch (popupType)
        {
            case PopupType.Item:
                itemPopupController.Open();
                break;

            case PopupType.Upgrade:
                upgradePopupController.Open();
                break;

            case PopupType.Sell:
                selectionController.ToggleMode(SelectionMode.Multi);
                sellPopupContoroller.gameObject.SetActive(true);
                break;
        }
        itemPopupController.UpdatePopupUI();
    }
}
