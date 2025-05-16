using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템 팝업 UI를 관리하는 클래스입니다.
/// </summary>
public class ItemPopupUI : PopupBase
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private Inventory inventory;
    private Equipment equipment;
    private ItemActionHandler itemActionHandler;
    private ItemSelectionController selectionController;
    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;

    private ItemInstance itemInstance;
    public Action equipAction;

    public override void Init()
    {
        base.Init();
        var mainSceneManager = MainSceneManager.Instance;
        inventory = mainSceneManager.inventory;
        equipment = mainSceneManager.equipment;
        itemActionHandler = mainSceneManager.inventoryManager.itemActionHandler;
        selectionController = mainSceneManager.inventoryManager.inventorySelectionController;
        equipmentSlotContainer = mainSceneManager.inventoryManager.equipmentSlotContainer;
        inventorySlotContainer = mainSceneManager.inventoryManager.inventorySlotContainer;

        

        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);

        UpdatePopupUI();
    }

    public override void Open()
    {
        itemInstance = selectionController.selectedData;
        if (itemInstance == null) return;
        base.Open();
        SetViewActive(true);
    }

    public override void Close()
    {
        SetViewActive(false);
        base.Close();
    }

    private void SetViewActive(bool active)
    {
        gameObject.SetActive(active);
        icon.gameObject.SetActive(active);
        if(!active)
        {
            itemName.text = string.Empty;
            description.text = string.Empty;
            return;
        }
        UpdatePopupUI();
    }

    public void UpdatePopupUI()
    {
        NeedInit();
        if (TryClearIfInventoryEmpty()) return;

        inventorySlotContainer.Refresh();
        equipmentSlotContainer.Refresh();

        if (itemInstance == null) return;

        icon.sprite = itemInstance.Data.Icon;
        itemName.text = itemInstance.Data.ItemName;
        description.text = itemInstance.Data.ItemDescript;

        bool isEquipped = equipment.IsEquipped(itemInstance);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);

        equipAction?.Invoke();
    }

    private bool TryClearIfInventoryEmpty()
    {
        if (inventory.GetAll().Count == 0)
        {
            SetViewActive(false);
            return true;
        }
        return false;
    }

    public void OnClickEquip()
    {
        itemActionHandler.Equip(itemInstance);
        UpdatePopupUI();

        if (itemInstance.AsEquipData.equipType == EquipType.Weapon)
            AnalyticsLogger.LogUserEquip(itemInstance.Data.ItemIndex, PlayerPrefs.GetInt("IdleMaxWave", 0));
    }

    public void OnClickUnEquip()
    {
        itemActionHandler.UnEquip(itemInstance);
        equipmentSlotContainer.ClearSlot(itemInstance.AsEquipData.equipType);
        UpdatePopupUI();
    }
}

