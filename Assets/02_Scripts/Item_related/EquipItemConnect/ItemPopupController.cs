using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 아이템 팝업 UI를 관리하는 클래스입니다.
/// </summary>
public class ItemPopupController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button sellButton;

    private Equipment equipment;
    private Inventory inventory;
    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;
    private SelectionController selectionController;
    private UpgradePopup upgradePopup;

    public ItemInstance currentData { get; private set; }

    public Action<ItemInstance> OnItemSelected;
    public Action clickToUpdateText;

    public void Init()
    {
        var home = MainSceneManager.Instance;
        equipment = home.equipment;
        inventory = home.inventory;
        equipmentSlotContainer = home.inventoryGroup.equipmentSlotContainer;
        inventorySlotContainer = home.inventoryGroup.inventorySlotContainer;
        selectionController = home.inventoryGroup.selectionController;
        upgradePopup = home.inventoryGroup.upgradePopup;

        upgradeButton.onClick.AddListener(OnClickUpgrade);
        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);
        sellButton.onClick.AddListener(OnClickSell);
        if (FirstState()) return;
        SetData(inventory.GetAll()[inventory.GetAll().Count - 1]);
        selectionController.RefreshSlot(currentData);
        UpdatePopupUI();
    }

    private bool FirstState()
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
    /// <summary>
    /// 인벤토리 슬롯에서 아이템을 선택했을 때 호출됩니다. 액션을 위한 메서드입니다.
    /// </summary>
    /// <param name="instance"></param>
    public void SetData(ItemInstance instance)
    {
        if (instance == null) return;
        if (inventory.GetAll().Count == 0) { FirstState(); return; }
        icon.gameObject.SetActive(true);
        currentData = instance;
        OnItemSelected?.Invoke(currentData);

    }
    /// <summary>
    /// 인벤토리 슬롯에서 아이템을 선택했을 때 호출됩니다. 팝업을 열기 위한 메서드입니다.
    /// </summary>
    /// <param name="slot"></param>
    public void Open(Slot slot)
    {
        SetData(slot.GetData());
        icon.gameObject.SetActive(true);
        root.SetActive(true);
        UpdatePopupUI();
    }
    public void Open()
    {
        root.SetActive(true);
        UpdatePopupUI();
    }
    /// <summary>
    /// 팝업을 닫습니다.
    /// </summary>
    public void Close()
    {
        root.SetActive(false);
    }

    public void Clear()
    {
        currentData = null;
        icon.gameObject.SetActive(false);
        itemName.text = string.Empty;
        description.text = string.Empty;

    }
    /// <summary>
    /// 팝업 UI를 업데이트합니다. 아이템의 정보를 표시합니다.
    /// </summary>
    public void UpdatePopupUI()
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
        clickToUpdateText?.Invoke();
    }
    /// <summary>
    /// 업그레이드 버튼 클릭 시 호출됩니다. 아이템을 업그레이드합니다.
    /// </summary>
    public void OnClickUpgrade()
    {
        if (currentData == null || currentData.AsEquipData == null) return;
        upgradePopup.gameObject.SetActive(true);
    }

    public void OnClickSell()
    {
        if(equipment.IsEquipped(currentData)) equipment.UnEquip(currentData);
        inventory.RemoveItem(currentData);
        GameManager.Instance.gold += currentData.Data.ItemSellPrice;
        SaveSystem.SaveGame();
        UpdatePopupUI();
        Clear();
        Close();
    }
    /// <summary>
    /// 장비 버튼 클릭 시 호출됩니다. 아이템을 장착합니다.
    /// </summary>
    public void OnClickEquip()
    {
        if (currentData?.AsEquipData != null)
        {
            equipment.Equip(currentData);
            UpdatePopupUI();
        }
    }
    /// <summary>
    /// 장비 해제 버튼 클릭 시 호출됩니다. 아이템을 해제합니다.
    /// </summary>
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

