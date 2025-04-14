using System;
using System.Collections.Generic;

public class Inventory
{
    private List<ItemData> inventory = new();
    private EquipType currentType = EquipType.Count;

    public event Action OnInventoryChanged;

    public void AddItem(ItemData item)
    {
        inventory.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item)
    {
        inventory.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public void SetType(EquipType type)
    {
        if (currentType == type) return;
        currentType = type;
        OnInventoryChanged?.Invoke();
    }

    public void ClearAll()
    {
        inventory.Clear();
        OnInventoryChanged?.Invoke();
    }

    public IReadOnlyList<ItemData> GetFilteredView()
    {
        var viewList = currentType == EquipType.Count
            ? new List<ItemData>(inventory)
            : inventory.FindAll(x => x is EquipData y && y.equipType == currentType);

        viewList.Sort((a, b) => b.itemGrade.CompareTo(a.itemGrade));
        return viewList;
    }

    public IReadOnlyList<ItemData> ModifyAndGetFiltered(Action<Inventory> modification)
    {
        if (modification == null)
            throw new ArgumentNullException(nameof(modification));

        modification(this);
        return GetFilteredView();
    }

    public IReadOnlyList<ItemData> GetAll() => inventory;
}