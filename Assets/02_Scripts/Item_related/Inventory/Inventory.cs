using System;
using System.Collections.Generic;

public enum InventorySortType
{
    Grade,
    Name,
    Recent
}
/// <summary>
/// 인벤토리 클래스입니다.
/// </summary>
public class Inventory
{
    private List<ItemInstance> inventory = new();

    private InventorySortType currentSortType = InventorySortType.Grade;
    private List<ItemType> currentFilter = new() { ItemType.Equipment };
    private List<EquipType> currentEquipFilter = new() { EquipType.Count };

    private Dictionary<InventorySortType, bool> sortDirections = new()
    {
        { InventorySortType.Grade, true },
        { InventorySortType.Name, false },
        { InventorySortType.Recent, true }
    };

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
        else
        {
            var target = inventory.Find(i => i.UniqueID == item.UniqueID);
            if (target != null) inventory.Remove(target);
        }

        if (updateUI) OnInventoryChanged?.Invoke();
    }

    public void ClearAll()
    {
        inventory.Clear();
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

    public void SetSortType(InventorySortType type, bool isDescending)
    {
        currentSortType = type;
        sortDirections[type] = isDescending;
        OnInventoryChanged?.Invoke();
    }

    public bool GetSortDirection(InventorySortType type)
    {
        return sortDirections.TryGetValue(type, out var dir) && dir;
    }


    private Comparison<ItemInstance> GetComparison(InventorySortType sortType, bool descending)
    {
        Comparison<ItemInstance> cmp = sortType switch
        {
            InventorySortType.Grade => (a, b) => a.Data.ItemGrade.CompareTo(b.Data.ItemGrade),
            InventorySortType.Name => (a, b) => string.Compare(a.Data.ItemName, b.Data.ItemName, StringComparison.Ordinal),
            InventorySortType.Recent => (a, b) => a.UniqueID.CompareTo(b.UniqueID),
            _ => (a, b) => 0
        };

        return descending ? (a, b) => -cmp(a, b) : cmp;
    }

    private void MultiSort(List<ItemInstance> list)
    {
        List<InventorySortType> priorities = new();

        foreach (InventorySortType type in Enum.GetValues(typeof(InventorySortType)))
        {
            if (type == currentSortType)
                priorities.Insert(0, type);
            else
                priorities.Add(type);
        }

        list.Sort((a, b) =>
        {
            foreach (var type in priorities)
            {
                var cmp = GetComparison(type, GetSortDirection(type));
                int result = cmp(a, b);
                if (result != 0)
                    return result;
            }
            return 0;
        });
    }

    /// <summary>
    /// 인벤토리의 필터링된 뷰를 반환합니다. 필터링된 아이템을 정렬합니다.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<ItemInstance> GetFilteredView()
    {
        List<ItemInstance> viewList = FilterList();
        MultiSort(viewList);
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
