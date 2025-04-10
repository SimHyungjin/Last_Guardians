using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 재고 화면과 관계된 데이터 전략 관리처.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer slotContainer;

    [SerializeField] private List<ItemData> inventory;
    [SerializeField] private EquipType currentType = EquipType.Count;

    private void Awake()
    {
        inventory ??= new();
    }

    private void Start()
    {
        slotContainer.Init();
        UpdateFilteredView();
    }

    public void AddItem(ItemData itemdata)
    {
        inventory.Add(itemdata);
        UpdateFilteredView();
    }

    public void RemoveItem(ItemData itemdata)
    {
        inventory.Remove(itemdata);
        UpdateFilteredView();
    }

    public void SetType(EquipType type)
    {
        if (currentType == type) return;
        currentType = type;
        HomeManager.Instance.selectionController.DeselectSlot();
        UpdateFilteredView();
    }

    public void UpdateFilteredView()
    {
        var viewList = currentType == EquipType.Count ? new List<ItemData>(inventory) : inventory.FindAll(x => x is EquipmentData y && y.equipType == currentType);
        viewList.Sort((a, b) => b.itemGrade.CompareTo(a.itemGrade));
        slotContainer.Display(viewList);
    }

    public IReadOnlyList<ItemData> GetAll() => inventory;
}
