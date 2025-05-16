using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템 팝업 UI를 관리하는 클래스입니다.
/// </summary>
public class ItemPopupController : PopupBase
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private Equipment equipment;
    private Inventory inventory;
    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;

    public ItemInstance currentData { get; private set; }

    public Action<ItemInstance> OnItemSelected;
    public Action OnItemPopupUIUpdate;

    public override void Init()
    {
        base.Init();
        var mainSceneManager = MainSceneManager.Instance;
        equipment = mainSceneManager.equipment;
        inventory = mainSceneManager.inventory;
        equipmentSlotContainer = mainSceneManager.inventoryGroup.equipmentSlotContainer;
        inventorySlotContainer = mainSceneManager.inventoryGroup.inventorySlotContainer;

        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);

        UpdatePopupUI();
    }

    public override void Open()
    {
        base.Open();
        if (currentData == null) return;
        icon.gameObject.SetActive(true);
        root.SetActive(true);
        UpdatePopupUI();
    }

    public override void Close()
    {
        base.Close();

        currentData = null;
        icon.gameObject.SetActive(false);
        itemName.text = string.Empty;
        description.text = string.Empty;
        root.SetActive(false);
    }

    public void SetData(ItemInstance instance)
    {
        NeedInit();
        if (instance == null) return;
        if (TryClearIfInventoryEmpty()) return;
        currentData = instance;
    }

    public void UpdatePopupUI()
    {
        NeedInit();
        if (TryClearIfInventoryEmpty()) return;

        inventorySlotContainer.Display(inventory.GetFilteredView());
        equipmentSlotContainer.Refresh();

        if (currentData == null) return;

        icon.sprite = currentData.Data.Icon;
        itemName.text = currentData.Data.ItemName;
        description.text = currentData.Data.ItemDescript;

        bool isEquipped = equipment.IsEquipped(currentData);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);

        OnItemSelected?.Invoke(currentData);
        OnItemPopupUIUpdate?.Invoke();
    }

    private bool TryClearIfInventoryEmpty()
    {
        if (inventory.GetAll().Count == 0)
        {
            currentData = null;
            icon.gameObject.SetActive(false);
            itemName.text = string.Empty;
            description.text = string.Empty;
            return true;
        }
        return false;
    }

    public void OnClickEquip()
    {
        if (currentData?.AsEquipData == null) return;
        equipment.Equip(currentData);

        if (currentData.AsEquipData.equipType == EquipType.Weapon)
            AnalyticsLogger.LogUserEquip(currentData.Data.ItemIndex, PlayerPrefs.GetInt("IdleMaxWave", 0));
        UpdatePopupUI();
    }

    public void OnClickUnEquip()
    {
        if (currentData?.AsEquipData == null) return;
        equipment.UnEquip(currentData);
        equipmentSlotContainer.ClearSlot(currentData.AsEquipData.equipType);
        UpdatePopupUI();
    }
}

