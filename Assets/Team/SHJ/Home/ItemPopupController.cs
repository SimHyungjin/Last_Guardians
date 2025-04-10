using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopupController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private Equipment equipment;
    private EquipmentData currentData;

    private void Start()
    {
        equipment = HomeManager.Instance.equipment;
        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);
        root.SetActive(false);
    }
    public void Open(Slot slot)
    {
        var data = slot.GetData();
        currentData = data as EquipmentData;
        UpdatePopupUI();

        root.SetActive(true);
    }

    public void OnClickEquip()
    {
<<<<<<< Updated upstream:Assets/Team/SHJ/Home/ItemPopupController.cs
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
=======
        currentData = null;
        root.SetActive(false);
    }

    public void UpdatePopupUI()
    {
        if (currentData == null) return;
        icon.sprite = currentData.icon;
        itemName.text = currentData.itemName;
        description.text = currentData.itemDescription;

        if(currentData is not EquipmentData data) return;
        bool isEquipped = equipment.IsEquipped(currentData);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);
    }


    public void OnClickEquip()
    {
        if (currentData != null)
        {
            equipment.Equip(currentData);
            HomeManager.Instance.inventory.UpdateFilteredView();
            HomeManager.Instance.equipmentSlotContainer.BindEquipment(currentData.equipType, currentData);
            UpdatePopupUI();
        }
    }

    public void OnClickUnEquip()
    {
        if (currentData != null)
        {
            equipment.UnEquip(currentData);
            HomeManager.Instance.inventory.UpdateFilteredView();
            HomeManager.Instance.equipmentSlotContainer.ClearSlot(currentData.equipType);
            UpdatePopupUI();
        }
>>>>>>> Stashed changes:Assets/02_Scripts/HomeUI/Home/ItemPopupController.cs
    }
}
