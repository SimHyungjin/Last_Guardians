using UnityEngine;
using UnityEngine.UI;

public class ItemPopupController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text description;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private ItemData currentData;

    public void Open(ItemData data)
    {
        currentData = data;
        icon.sprite = data.icon;
        itemName.text = data.itemName;
        description.text = data.itemDescription;

        if (data is EquipemntData equipData)
        {
            bool isEquipped = HomeManager.Instance.equipment.IsEquipped(equipData);
            equipButton.gameObject.SetActive(!isEquipped);
            unequipButton.gameObject.SetActive(isEquipped);
        }
        else
        {
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(false);
        }

        root.SetActive(true);
    }

    public void OnClickEquip()
    {
        if (currentData is EquipemntData equipData)
        {
            HomeManager.Instance.equipment.Equip(equipData);
        }
        Close();
    }

    public void OnClickUnEquip()
    {
        if (currentData is EquipemntData equipData)
        {
            HomeManager.Instance.equipment.UnEquip(equipData);
        }
        Close();
    }

    public void Close()
    {
        HomeManager.Instance.inventory.UpdateCurInventory();
        root.SetActive(false);
    }
}
