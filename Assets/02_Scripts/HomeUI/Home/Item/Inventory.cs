using System;
using System.Collections.Generic;

public class Inventory
{
    public enum InventorySortType
    {
        GradeDescending,
        NameAscending,
        Recent
    }

    private List<ItemInstance> inventory = new();

    private InventorySortType currentSortType = InventorySortType.GradeDescending;
    private ItemType currentFilter = ItemType.Equipment;
    private EquipType currentEquipFilter = EquipType.Count;

    public event Action OnInventoryChanged;

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
        if(updateUI) OnInventoryChanged?.Invoke();
    }

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

        if(updateUI) OnInventoryChanged?.Invoke();
    }

    public void SetSortType(InventorySortType sortType)
    {
        if (currentSortType == sortType) return;
        currentSortType = sortType;
        OnInventoryChanged?.Invoke();
    }

    public void SetItemType(ItemType type)
    {
        if (currentFilter == type) return;
        currentFilter = type;
        OnInventoryChanged?.Invoke();
    }

    public void SetEquipTypeFilter(EquipType type)
    {
        if (currentEquipFilter == type) return;
        currentEquipFilter = type;
        OnInventoryChanged?.Invoke();
    }

    public void ClearAll(bool updateUI = true)
    {
        inventory.Clear();
        if (updateUI) OnInventoryChanged?.Invoke();
    }

    public IReadOnlyList<ItemInstance> GetFilteredView()
    {
        List<ItemInstance> viewList = inventory.FindAll(x =>
        {
            if (x.Data.ItemType != currentFilter) return false;

            if (currentFilter == ItemType.Equipment && x.AsEquipData != null)
            {
                return currentEquipFilter == EquipType.Count || x.AsEquipData.equipType == currentEquipFilter;
            }

            return true;
        });

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

    public IReadOnlyList<ItemInstance> ModifyAndGetFiltered(Action<Inventory> modification)
    {
        if (modification == null) return default;

        modification(this);
        return GetFilteredView();
    }

    public IReadOnlyList<ItemInstance> GetAll() => inventory;
}
