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

    [SerializeField] private Button upgradePopupOpenButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button sellMoneyButton;
    [SerializeField] private Button sellStoneButton;

    private Equipment equipment;
    private Inventory inventory;
    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;
    private SelectionController selectionController;
    private UpgradePopup upgradePopup;

    public ItemInstance currentData { get; private set; }

    public Action<ItemInstance> OnEnableCurretData;
    public Action OnItemPopupUIUpdate;

    public void Init()
    {
        var home = MainSceneManager.Instance;
        equipment = home.equipment;
        inventory = home.inventory;
        equipmentSlotContainer = home.inventoryGroup.equipmentSlotContainer;
        inventorySlotContainer = home.inventoryGroup.inventorySlotContainer;
        selectionController = home.inventoryGroup.selectionController;
        upgradePopup = home.inventoryGroup.upgradePopup;

        upgradePopupOpenButton.onClick.AddListener(OnClickUpgradePopupOpen);
        equipButton.onClick.AddListener(OnClickEquip);
        unequipButton.onClick.AddListener(OnClickUnEquip);
        sellMoneyButton.onClick.AddListener(() => OnClickSell(true));
        //sellStoneButton.onClick.AddListener(() => OnClickSell(false));
        if (InventroyEmptyCheck()) return;
        SetData(inventory.GetAll()[inventory.GetAll().Count - 1]);
        selectionController.RefreshSlot(currentData);
        UpdatePopupUI();
    }

    private bool InventroyEmptyCheck()
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
        if (InventroyEmptyCheck()) return;
        currentData = instance;
        OnEnableCurretData?.Invoke(currentData);
    }
    /// <summary>
    /// 인벤토리 슬롯에서 아이템을 선택했을 때 호출됩니다. 팝업을 열기 위한 메서드입니다.
    /// </summary>
    /// <param name="slot"></param>
    public void Open()
    {
        if(currentData == null) return;
        icon.gameObject.SetActive(true);
        root.SetActive(true);
        UpdatePopupUI();
    }

    /// <summary>
    /// 팝업을 닫습니다.
    /// </summary>
    public void Close()
    {
        currentData = null;
        icon.gameObject.SetActive(false);
        itemName.text = string.Empty;
        description.text = string.Empty;
        root.SetActive(false);
    }

    /// <summary>
    /// 팝업 UI를 업데이트합니다. 아이템의 정보를 표시합니다.
    /// </summary>
    public void UpdatePopupUI()
    {
        inventorySlotContainer.Display(inventory.GetFilteredView());
        equipmentSlotContainer.Refresh();

        if (currentData == null) 
        {   
            Close(); return;
        }
        icon.sprite = currentData.Data.icon;
        itemName.text = currentData.Data.itemName;
        description.text = currentData.Data.ItemDescript;

        upgradePopupOpenButton.gameObject.SetActive(currentData.Data.itemGrade < ItemGrade.Legend);

        bool isEquipped = equipment.IsEquipped(currentData);
        equipButton.gameObject.SetActive(!isEquipped);
        unequipButton.gameObject.SetActive(isEquipped);
        OnItemPopupUIUpdate?.Invoke();
    }
    /// <summary>
    /// 업그레이드 버튼 클릭 시 호출됩니다. 아이템을 업그레이드합니다.
    /// </summary>
    public void OnClickUpgradePopupOpen()
    {
        if (currentData == null || currentData?.AsEquipData == null) return;
        upgradePopup.Init(currentData);
        upgradePopup.gameObject.SetActive(true);
    }

    public void OnClickSell(bool money)
    {
        if (currentData == null || currentData?.AsEquipData == null) return;
        if (equipment.IsEquipped(currentData)) equipment.UnEquip(currentData);
        inventory.RemoveItem(currentData);
        if(money)
        {
            GameManager.Instance.gold += currentData.Data.ItemSellPrice;
        }
        else
        {
            //GameManager.Instance.upgradeStones += currentData.Data.ItemUpgradeStone;
        }
        SaveSystem.SaveGame();
        Close();
    }
    /// <summary>
    /// 장비 버튼 클릭 시 호출됩니다. 아이템을 장착합니다.
    /// </summary>
    public void OnClickEquip()
    {
        if (currentData == null || currentData?.AsEquipData == null) return;
        equipment.Equip(currentData);
        UpdatePopupUI();
    }
    /// <summary>
    /// 장비 해제 버튼 클릭 시 호출됩니다. 아이템을 해제합니다.
    /// </summary>
    public void OnClickUnEquip()
    {
        if (currentData == null || currentData?.AsEquipData == null) return;
        equipment.UnEquip(currentData);
        equipmentSlotContainer.ClearSlot(currentData.AsEquipData.equipType);
        UpdatePopupUI();
    }
}

