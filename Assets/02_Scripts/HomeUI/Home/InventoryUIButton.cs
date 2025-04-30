using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIButton : MonoBehaviour
{
    [field: Header("정렬 버튼")]
    [field: SerializeField] public Button sortByGradeBtn { get; private set; }
    [field: SerializeField] public Button sortByNameBtn { get; private set; }
    [field: SerializeField] public Button sortByRecentBtn { get; private set; }

    [field: Header("ItemType 필터 버튼")]
    [field: SerializeField] public Button allTypeBtn { get; private set; }
    [field: SerializeField] public Button equipmentBtn { get; private set; }
    [field: SerializeField] public Button upgradeStoneBtn { get; private set; }

    [field: Header("EquipType 필터 버튼 (장비 세부 필터)")]
    [field: SerializeField] public Button allEquipBtn { get; private set; }
    [field: SerializeField] public Button weaponBtn { get; private set; }
    [field: SerializeField] public Button helmetBtn { get; private set; }
    [field: SerializeField] public Button armorBtn { get; private set; }
    [field: SerializeField] public Button shoesBtn { get; private set; }
    [field: SerializeField] public Button ringBtn { get; private set; }
    [field: SerializeField] public Button necklaceBtn { get; private set; }

    public UnityEvent<InventorySortType> onSortButtonClicked;
    public UnityEvent<ItemType> onItemTypeFilter;
    public UnityEvent<EquipType> onEquipTypeFilter;

    private void Awake()
    {
        sortByGradeBtn.onClick.AddListener(() => onSortButtonClicked?.Invoke(InventorySortType.GradeDescending));
        sortByNameBtn.onClick.AddListener(() => onSortButtonClicked?.Invoke(InventorySortType.NameAscending));
        sortByRecentBtn.onClick.AddListener(() => onSortButtonClicked?.Invoke(InventorySortType.Recent));

        allTypeBtn.onClick.AddListener(() => { onEquipTypeFilter?.Invoke(EquipType.Count); onItemTypeFilter?.Invoke(ItemType.Count); });
        equipmentBtn.onClick.AddListener(() => { onEquipTypeFilter?.Invoke(EquipType.Count); onItemTypeFilter?.Invoke(ItemType.Equipment); });
        upgradeStoneBtn.onClick.AddListener(() => onItemTypeFilter?.Invoke(ItemType.UpgradeStone));

        EquipBtnListener(allEquipBtn, EquipType.Count);
        EquipBtnListener(weaponBtn, EquipType.Weapon);
        EquipBtnListener(helmetBtn, EquipType.Helmet);
        EquipBtnListener(armorBtn, EquipType.Armor);
        EquipBtnListener(shoesBtn, EquipType.Shoes);
        EquipBtnListener(ringBtn, EquipType.Ring);
        EquipBtnListener(necklaceBtn, EquipType.Necklace);
    }

    private void EquipBtnListener(Button button, EquipType equipType)
    {
        button.onClick.AddListener(() =>
        {
            onItemTypeFilter?.Invoke(ItemType.Equipment);
            onEquipTypeFilter?.Invoke(equipType);
        });
    }
}
