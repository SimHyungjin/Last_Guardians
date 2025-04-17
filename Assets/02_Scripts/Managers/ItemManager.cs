using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    [SerializeField] private Dictionary<int, ItemData> itemDatas = new();

    public void LoadAllItems()
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("SO/Equip");
        foreach (ItemData item in allItems)
        {
            itemDatas[item.ItemIndex] = item;
        }
    }

    public ItemInstance GetItemInstanceByIndex(int index)
    {
        return itemDatas.TryGetValue(index, out var data)? new ItemInstance(data): null;
    }
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