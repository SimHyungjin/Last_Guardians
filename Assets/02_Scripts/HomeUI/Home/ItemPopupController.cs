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
        currentData = slot.GetData() as EquipmentData;
        UpdatePopupUI();
        root.SetActive(true);
    }

    public void Close()
    {
        currentData = null;
        root.SetActive(false);
    }

    private void UpdatePopupUI()
    {
        if (currentData == null) return;
        icon.sprite = currentData.icon;
        itemName.text = currentData.itemName;
        description.text = currentData.itemDescription;

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
    }
}
