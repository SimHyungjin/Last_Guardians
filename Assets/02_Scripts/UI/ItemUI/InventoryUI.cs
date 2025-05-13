using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 UI의 버튼을 관리하는 클래스입니다.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [field: Header("재화 UI 버튼")]
    [field: SerializeField] public TextMeshProUGUI goldText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI upgradeStoneText { get; private set; }

    [field: Header("정렬 버튼")]
    [field: SerializeField] public Button sortByGradeBtn { get; private set; }
    [field: SerializeField] public Button sortByNameBtn { get; private set; }
    [field: SerializeField] public Button sortByRecentBtn { get; private set; }

    [field: Header("정렬 선택 판넬 버튼")]
    [field: SerializeField] public Button filterSelectBtn { get; private set; }
    [field: SerializeField] public GameObject filterSelectPanel { get; private set; }

    [field: Header("판매/분해 선택 버튼")]
    [field: SerializeField] public Button sellBtn { get; private set; }

    [field: Header("강화 버튼")]
    [field: SerializeField] public Button upgradeBtn { get; private set; }

    [field: Header("필터 버튼")]
    [field: SerializeField] public Button allTypeBtn { get; private set; }
    [field: SerializeField] public Button weaponTypeBtn { get; private set; }
    [field: SerializeField] public Button armorTypeBtn { get; private set; }
    [field: SerializeField] public Button accessorieTypeBtn { get; private set; }
    [field: SerializeField] public Button upgradeStoneTypeBtn { get; private set; }

    public Action<InventorySortType> onSortButtonClicked;
    public Action<ItemType[]> onItemTypeFilter;
    public Action<EquipType[]> onEquipTypeFilter;

    private readonly ItemType[] EquipmentOnly = { ItemType.Equipment };
    private readonly ItemType[] UpgradeStoneOnly = { ItemType.UpgradeStone };
    private readonly ItemType[] AllTypes = { ItemType.Count };

    private readonly EquipType[] AllEquip = { EquipType.Count };
    private readonly EquipType[] WeaponOnly = { EquipType.Weapon };
    private readonly EquipType[] ArmorTypes = { EquipType.Armor, EquipType.Helmet, EquipType.Shoes };
    private readonly EquipType[] AccessoryTypes = { EquipType.Ring, EquipType.Necklace };

    public void Init()
    {
        var itemConnecter = MainSceneManager.Instance.inventoryGroup.itemConnecter;

        itemConnecter.itemPopupController.OnItemPopupUIUpdate += RefreshGoods;
        itemConnecter.sellPopupController.OnSellAction += RefreshGoods;

        sortByGradeBtn.onClick.AddListener(() => onSortButtonClicked?.Invoke(InventorySortType.GradeDescending));
        sortByNameBtn.onClick.AddListener(() => onSortButtonClicked?.Invoke(InventorySortType.NameAscending));
        sortByRecentBtn.onClick.AddListener(() => onSortButtonClicked?.Invoke(InventorySortType.Recent));

        filterSelectBtn.onClick.AddListener(() => filterSelectPanel.SetActive(!filterSelectPanel.activeSelf));

        allTypeBtn.onClick.AddListener(() =>
        {
            onItemTypeFilter?.Invoke(AllTypes);
            onEquipTypeFilter?.Invoke(AllEquip);
        });

        weaponTypeBtn.onClick.AddListener(() =>
        {
            onItemTypeFilter?.Invoke(EquipmentOnly);
            onEquipTypeFilter?.Invoke(WeaponOnly);
        });

        armorTypeBtn.onClick.AddListener(() =>
        {
            onItemTypeFilter?.Invoke(EquipmentOnly);
            onEquipTypeFilter?.Invoke(ArmorTypes);
        });

        accessorieTypeBtn.onClick.AddListener(() =>
        {
            onItemTypeFilter?.Invoke(EquipmentOnly);
            onEquipTypeFilter?.Invoke(AccessoryTypes);
        });

        upgradeStoneTypeBtn.onClick.AddListener(() =>
        {
            onItemTypeFilter?.Invoke(UpgradeStoneOnly);
            onEquipTypeFilter?.Invoke(AllEquip);
        });

        sellBtn.onClick.AddListener(() => itemConnecter.OpenPopup(PopupType.Sell));
        upgradeBtn.onClick.AddListener(() => itemConnecter.OpenPopup(PopupType.Upgrade));

        RefreshGoods();
    }

    private void RefreshGoods()
    {
        goldText.text = GameManager.Instance.gold.ToString();
        upgradeStoneText.text = GameManager.Instance.upgradeStones.ToString();
    }
}
