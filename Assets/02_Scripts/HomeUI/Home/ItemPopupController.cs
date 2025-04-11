using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopupController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private Equipment equipment;
    private Inventory inventory;

    private EquipmentData currentData;

    private void Start()
    {
        equipment = HomeManager.Instance.equipment;
        inventory = HomeManager.Instance.inventory;
        upgradeButton.onClick.AddListener(OnClickUpgrade);
        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);
        root.SetActive(false);
    }

    public void Open(Slot slot)
    {
        currentData = slot.GetData() as EquipmentData;
        root.SetActive(true);
        UpdatePopupUI();
        
    }

    public void Close()
    {
        currentData = null;
        root.SetActive(false);
    }

    private void UpdatePopupUI()
    {
        if (currentData == null) return;
        HomeManager.Instance.inventorySlotContainer.Display(inventory.GetFilteredView());
        HomeManager.Instance.equipmentSlotContainer.Refresh();
        icon.sprite = currentData.icon;
        itemName.text = currentData.itemName;
        description.text = currentData.itemDescription;

        upgradeButton.gameObject.SetActive(currentData.itemGrade < ItemGrade.Legend);
        bool isEquipped = equipment.IsEquipped(currentData);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);
    }

    public void OnClickUpgrade()
    {
        if (currentData != null)
        {
            HomeManager.Instance.upgrade.TryUpgarade(currentData);
            UpdatePopupUI();
        }
    }
    public void OnClickEquip()
    {
        if (currentData != null)
        {
            equipment.Equip(currentData);
            UpdatePopupUI();
        }
    }

    public void OnClickUnEquip()
    {
        if (currentData != null)
        {
            equipment.UnEquip(currentData);
            HomeManager.Instance.equipmentSlotContainer.ClearSlot(currentData.equipType);
            UpdatePopupUI();
        }
    }
}
