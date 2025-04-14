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
    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;
    private SelectionController selectionController;

    private EquipData currentData;

    private void Start()
    {
        var home = HomeManager.Instance;
        equipment = home.equipment;
        inventory = home.inventory;
        equipmentSlotContainer = home.equipmentSlotContainer;
        inventorySlotContainer = home.inventorySlotContainer;
        selectionController = home.selectionController;

        upgradeButton.onClick.AddListener(OnClickUpgrade);
        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);
        root.SetActive(false);
    }

    public void Open(Slot slot)
    {
        currentData = slot.GetData() as EquipData;
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

        inventorySlotContainer.Display(inventory.GetFilteredView());
        equipmentSlotContainer.Refresh();

        icon.sprite = currentData.icon;
        itemName.text = currentData.itemName;
        description.text = currentData.ItemDescript;

        upgradeButton.gameObject.SetActive(currentData.itemGrade < ItemGrade.Legend);

        bool isEquipped = equipment.IsEquipped(currentData);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);
    }

    public void OnClickUpgrade()
    {
        if (currentData == null) return;

        if (HomeManager.Instance.upgrade.TryUpgrade(currentData, out var result))
        {
            inventory.RemoveItem(currentData);
            inventory.AddItem(result);

            selectionController.RefreshSlot(result);

            if (equipment.IsEquipped(currentData))
                equipment.Equip((EquipData)result);

            currentData = (EquipData)result;
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
            equipmentSlotContainer.ClearSlot(currentData.equipType);
            UpdatePopupUI();
        }
    }
}

