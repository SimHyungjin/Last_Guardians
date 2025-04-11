using System;
using System.Collections.Generic;

public class Inventory
{
    private List<ItemData> inventory = new();
    private EquipType currentType = EquipType.Count;

    public void AddItem(ItemData item)
    {
        inventory.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        inventory.Remove(item);
    }

    public void SetType(EquipType type)
    {
        if (currentType == type) return;
        currentType = type;
    }

    public void ClearAll()
    {
        inventory.Clear();
    }

    public IReadOnlyList<ItemData> GetFilteredView()
    {
        var viewList = currentType == EquipType.Count ? new List<ItemData>(inventory) : inventory.FindAll(x => x is EquipmentData y && y.equipType == currentType);
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
