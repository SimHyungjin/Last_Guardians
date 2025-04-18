using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ItemManager
{
    private Dictionary<int, ItemData> itemDatas = new();
    private ReadOnlyDictionary<int, ItemData> readonlyItemDatas;

    public void LoadAllItems()
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("SO/Equip");
        foreach (ItemData item in allItems)
        {
            itemDatas[item.ItemIndex] = item;
        }

        readonlyItemDatas = new ReadOnlyDictionary<int, ItemData>(itemDatas);
    }

    public ItemInstance GetItemInstanceByIndex(int index)
    {
        return itemDatas.TryGetValue(index, out var data)? new ItemInstance(data): null;
    }

    public ReadOnlyDictionary<int, ItemData> ItemDatas() => readonlyItemDatas;
}

public class ItemInstance
{
    public int UniqueID { get; private set; }
    public ItemData Data { get; private set; }
    public int Count { get; private set; } = 1;

    public EquipData AsEquipData => Data as EquipData;

    public ItemInstance(ItemData data)
    {
        Data = data;
        UniqueID = System.Guid.NewGuid().GetHashCode();
    }

    public void OverrideUniqueID(int id)
    {
        UniqueID = id;
    }

    public void AddCount(int amount) => Count += amount;
    public void SubtractCount(int amount) => Count = Mathf.Max(Count - amount, 0);
    public void SetCount(int value) => Count = value;
}