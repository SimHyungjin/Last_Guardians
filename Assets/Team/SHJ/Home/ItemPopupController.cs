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

    public void Close()
    {
        root.SetActive(false);
    }

    //public void OnClickEquip()
    //{
    //    HomeManager.Instance.equipment.Equip(currentData);
    //    Close();
    //}

    //public void OnClickUnEquip()
    //{
    //    Equipment.Instance.UnEquip(currentData);
    //    Close();
    //}
}
