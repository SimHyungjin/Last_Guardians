using System;
using System.Collections.Generic;

public enum InventorySortType
{
    GradeDescending,
    NameAscending,
    Recent
}
/// <summary>
/// 인벤토리 클래스입니다.
/// </summary>
public class Inventory
{
    private List<ItemInstance> inventory = new();

    private InventorySortType currentSortType = InventorySortType.GradeDescending;
    private List<ItemType> currentFilter = new() { ItemType.Equipment };
    private List<EquipType> currentEquipFilter = new() { EquipType.Count };

    public event Action OnInventoryChanged;

    /// <summary>
    /// 인벤토리에 아이템을 추가합니다. 같은 아이템이 있을 경우 수량을 증가시킵니다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <param name="updateUI"></param>
    public void AddItem(ItemInstance item, int count = 1, bool updateUI = true)
    {
        if (item == null) return;

        if (item.Data.ItemStackLimit > 1)
        {
            var existing = inventory.Find(i => i.Data.ItemIndex == item.Data.ItemIndex);
            if (existing != null) existing.AddCount(count);
            else inventory.Add(item);
        }
        else inventory.Add(item);
        if (updateUI) OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// 인벤토리에서 아이템을 제거합니다. 같은 아이템이 있을 경우 수량을 감소시킵니다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <param name="updateUI"></param>
    public void RemoveItem(ItemInstance item, int count = 1, bool updateUI = true)
    {
        if (item == null) return;
        if (item.Data.ItemStackLimit > 1)
        {
            var existing = inventory.Find(i => i.Data.ItemIndex == item.Data.ItemIndex);
            if (existing != null)
            {
                existing.SubtractCount(count);
                if (existing.Count <= 0) inventory.Remove(existing);
            }
        }
        else inventory.Remove(item);

        if (updateUI) OnInventoryChanged?.Invoke();
    }

    public void SetSortType(InventorySortType sortType)
    {
        if (currentSortType == sortType) return;
        currentSortType = sortType;
        OnInventoryChanged?.Invoke();
    }

    public void SetItemTypeFilter(params ItemType[] types)
    {
        currentFilter = new List<ItemType>(types);
        OnInventoryChanged?.Invoke();
    }

    public void SetEquipTypeFilter(params EquipType[] types)
    {
        currentEquipFilter = new List<EquipType>(types);
        OnInventoryChanged?.Invoke();
    }

    public void ClearAll()
    {
        inventory.Clear();
        OnInventoryChanged?.Invoke();
    }
    /// <summary>
    /// 인벤토리의 필터링된 뷰를 반환합니다. 필터링된 아이템을 정렬합니다.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<ItemInstance> GetFilteredView()
    {
        List<ItemInstance> viewList = FilterList();
        switch (currentSortType)
        {
            case InventorySortType.GradeDescending:
                viewList.Sort((a, b) => b.Data.ItemGrade.CompareTo(a.Data.ItemGrade));
                break;
            case InventorySortType.NameAscending:
                viewList.Sort((a, b) => string.Compare(a.Data.ItemName, b.Data.ItemName, StringComparison.Ordinal));
                break;
            case InventorySortType.Recent:
                viewList.Reverse();
                break;
        }

        return viewList;
    }

    /// <summary>
    /// 인벤토리의 필터링된 리스트를 반환합니다. 필터링된 아이템을 정렬하지 않습니다.
    /// </summary>
    /// <returns></returns>
    private List<ItemInstance> FilterList()
    {
        if (currentFilter.Count == 0 || currentFilter.Contains(ItemType.Count))
            return new List<ItemInstance>(inventory);

        return inventory.FindAll(x =>
        {
            if (!currentFilter.Contains(x.Data.ItemType))
                return false;

            if (x.Data.ItemType == ItemType.Equipment && x.AsEquipData != null)
            {
                return currentEquipFilter.Count == 0 || currentEquipFilter.Contains(EquipType.Count) || currentEquipFilter.Contains(x.AsEquipData.equipType);
            }

            return true;
        });
    }
    /// <summary>
    /// 메서드 체이닝을 위한 필터링된 뷰를 반환합니다.
    /// </summary>
    /// <param name="modification"></param>
    /// <returns></returns>
    public IReadOnlyList<ItemInstance> ModifyAndGetFiltered(Action<Inventory> modification)
    {
        if (modification == null) return default;

        modification(this);
        return GetFilteredView();
    }

    public IReadOnlyList<ItemInstance> GetAll() => inventory;
}
