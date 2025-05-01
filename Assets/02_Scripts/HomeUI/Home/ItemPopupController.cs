using System;
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

    public ItemInstance currentData { get; private set; }

    public Action<ItemInstance> OnItemSelected;

    public void Init()
    {
        var home = MainSceneManager.Instance;
        equipment = home.equipment;
        inventory = home.inventory;
        equipmentSlotContainer = home.inventoryGroup.equipmentSlotContainer;
        inventorySlotContainer = home.inventoryGroup.inventorySlotContainer;
        selectionController = home.inventoryGroup.selectionController;

        upgradeButton.onClick.AddListener(OnClickUpgrade);
        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);
        SetData(inventory.GetAll()[inventory.GetAll().Count-1]);
        selectionController.RefreshSlot(currentData);
        UpdatePopupUI();
        //root.SetActive(false);
    }

    public void SetData(ItemInstance instance)
    {
        if (instance == null) return;
        currentData = instance;
        OnItemSelected?.Invoke(currentData);

    }
    public void Open(Slot slot)
    {
        SetData(slot.GetData());
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
        if (currentData == null || currentData.AsEquipData == null) return;

        inventorySlotContainer.Display(inventory.GetFilteredView());
        equipmentSlotContainer.Refresh();

        icon.sprite = currentData.Data.icon;
        itemName.text = currentData.Data.itemName;
        description.text = currentData.Data.ItemDescript;

        upgradeButton.gameObject.SetActive(currentData.Data.itemGrade < ItemGrade.Legend);

        bool isEquipped = equipment.IsEquipped(currentData);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);
    }

    public void OnClickUpgrade()
    {
        if (currentData == null || currentData.AsEquipData == null) return;

        MainSceneManager.Instance.upgrade.TryUpgrade(currentData, out var result);

         SaveSystem.RemoveEquip(currentData.UniqueID);
        inventory.RemoveItem(currentData);

        SaveSystem.SaveEquipReward(result);
        inventory.AddItem(result);

        selectionController.RefreshSlot(result);

        if (equipment.IsEquipped(currentData))
            equipment.Equip(result);

        SetData(result);
        UpdatePopupUI();
    }

    public void OnClickEquip()
    {
        if (currentData?.AsEquipData != null)
        {
            equipment.Equip(currentData);
            UpdatePopupUI();
        }
    }

    public void OnClickUnEquip()
    {
        if (currentData?.AsEquipData != null)
        {
            equipment.UnEquip(currentData);
            equipmentSlotContainer.ClearSlot(currentData.AsEquipData.equipType);
            UpdatePopupUI();
        }
    }
}

