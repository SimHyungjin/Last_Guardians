using System;
using System.Collections.Generic;

public class Inventory
{
    private List<ItemInstance> inventory = new();
    private EquipType currentType = EquipType.Count;

    public event Action OnInventoryChanged;

    public void AddItem(ItemInstance item)
    {
        if (item == null) return;
        inventory.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemInstance item)
    {
        if (item == null) return;
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

    public IReadOnlyList<ItemInstance> GetFilteredView()
    {
        List<ItemInstance> viewList;

        if (currentType == EquipType.Count)
        {
            viewList = new List<ItemInstance>(inventory);
        }
        else
        {
            viewList = inventory.FindAll(x => x.asEquipData is EquipData ed && ed.equipType == currentType);
        }

        viewList.Sort((a, b) => b.Data.itemGrade.CompareTo(a.Data.itemGrade));

        return viewList;
    }

    public IReadOnlyList<ItemInstance> ModifyAndGetFiltered(Action<Inventory> modification)
    {
        if (modification == null)
            throw new ArgumentNullException(nameof(modification));

        modification(this);
        return GetFilteredView();
    }

    public IReadOnlyList<ItemInstance> GetAll() => inventory;
}
